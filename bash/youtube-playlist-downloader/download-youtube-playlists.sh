#!/bin/bash

googleAPIKey=""
SMTPServer="smtps://smtp.gmail.com:465"
emailRecipient=""
emailSenderUsername=""
emailSenderPassword=""
scriptLocation=""
playlistListLocation="./playlists"
youtubeDownloaderLocation="/usr/local/bin/youtube-dl" #/home/{YOUR_USER}/.local/bin/yt-dlp

cd ${scriptLocation}
status=$?
if [ ${status} -eq 1 ];
then
    echo "EXCEPTION: Could not move to scriptLocation specified."
    exit 1
fi

# Create the line for breaks between playlists
breakText=""
for i in {1..160};
do
    breakText+="-"
done

dateText=`date -I`
dateBlock="${breakText}\nDATE:\n${dateText}\n${breakText}"

emailText="From: ${emailSenderUsername}\nTo: ${emailRecipient}\nSubject: Weekly YTD video collection update\n\nDownloaded these new videos this week!\n${breakText}"

if ! test -f "${playlistListLocation}"; then
    echo -e ${dateBlock} >> ${scriptLocation}/error-log.txt
    echo -e "----------\nEXCEPTION: No playlist (called \"playlists\") text file data found... Exiting...: \n${playlist}\n----------" \
        >> ${scriptLocation}/error-log.txt
    exit 0
fi

# For each playlist in the playlist list
for playlist in `cat ${playlistListLocation}`;
do
    # Get the name of the playlist. First determine if we're dealing with a normal playlist, or a channel URL. 
    #If the URL is a normal playlist, request directly against the Google YouTube API. Otherwise, use YT-DL's built 
    #in way of retreiveing the playlist name.
    #Bugfix Note: When requesting for the playlist name, a premier video cannot be used because the YouTube API considers premier 
    #videos to not exist. Therefore, the above line's fix was implemented using --playlist-reverse in tandem 
    #with --playlist-end 2 and also -i to ignore errors in order to to try as hard as possible to ensure there is a 
    #video to extract the playlist name from. The assumption being that there is only one premier video uploaded at a time. 
    #There must be a cap on the amount of videos requested, otherwise the request takes an inordinate amount of time to 
    #complete, just to get one line of information.
    #Failing that, we're checking here to see if the playlist name exists, if it does not, then skip this playlist's download. 
    if [[ ${playlist} == *"list="* ]]; then
        #Use parameter expansion to strip the line of the uneeded info.
        playlistID=${playlist#*=}
        #GET request against the Google YouTube API to get the playlist name.
        playlistName=`curl -s -X GET \
            "https://www.googleapis.com/youtube/v3/playlists?key=${googleAPIKey}&id=${playlistID}&part=snippet&fields=items(snippet(title))" \
            --header "Accept: application/json" \
            | jq -r .items[].snippet.title`
    else
        #Get the name of the playlist by making a request and pulling the JSON information for the first 
        # video in the playlist
        playlistName=`${youtubeDownloaderLocation} --playlist-reverse --playlist-end 2 -i -j ${playlist} \
            | jq -r .playlist_title | head -n 1`
    fi
    
    if [ -z "${playlistName}" ]; then
        echo -e ${dateBlock} >> ${scriptLocation}/error-log.txt
        echo -e "----------\nEXCEPTION: No playlist-name data found... Skipping this playlist: \n${playlist}\n----------" \
            >> ${scriptLocation}/error-log.txt
        emailText+="\nERROR Downloading: See error-log.txt file for more information...\n${breakText}"
        continue
    fi

    #Add the current date to the log
    echo -e ${dateBlock} >> ${scriptLocation}/temp-log.txt

    # Download the videos
    ${youtubeDownloaderLocation} --download-archive ${scriptLocation}/"${playlistName}"/download-archive.txt \
    -o '%(playlist)s/%(playlist_index)s - %(title)s - %(id)s.%(ext)s' \
    --add-metadata -i -x -f bestaudio ${playlist} >> ${scriptLocation}/temp-log.txt

    # Get the newly added videos based on the output of the download, from the output, look for the 
    # unique line "[ffmpeg] Adding metadata to" that only occurs on a new download, then strip all 
    # information aside from video name
    metadataStringToRemove="\[ffmpeg\] Adding metadata to "
    newlyDownloadedSongs=`cat ${scriptLocation}/temp-log.txt | grep "$metadataStringToRemove" | sed "s/${metadataStringToRemove}//" \
        | sed "s/'\(.*\)'/\1/" | sed "s/.opus\|.m4a//" | sed "s/${playlistName}\///"`

    # If there are no new videos downloaded, change the output text
    if [[ -z "${newlyDownloadedSongs}" ]]
    then
        newlyDownloadedSongs+="No new songs for this playlist..."
    fi

    #Concatinate the strings to the main email text that will be sent in the email request
    emailText+="\nFor playlist: ${playlistName}\n${newlyDownloadedSongs}\n${breakText}"

    # Remove the temporary log file and put the output into the main log file
    cat ${scriptLocation}/temp-log.txt >> ${scriptLocation}/"${playlistName}"/log.txt
    rm ${scriptLocation}/temp-log.txt
done

# Get the used space for the directory
newDiskUsage=`du -hc`
emailText+="\nCurrent Disk Usage for the download directory:\n${newDiskUsage}"

# Send an email to the user
curl -s --url "${SMTPServer}" --ssl-reqd \
    --mail-from "${emailSenderUsername}" \
    --mail-rcpt "${emailRecipient}" \
    --user "${emailSenderUsername}:${emailSenderPassword}" \
    -T <(echo -e "${emailText}")
