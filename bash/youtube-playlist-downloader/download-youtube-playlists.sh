#!/bin/bash

SMTPServer="smtps://smtp.gmail.com:465"
emailRecipient=""
emailSenderUsername=""
emailSenderPassword=""
scriptLocation=""
playlistListLocation=""
youtubeDownloaderLocation=""

cd ${scriptLocation}
status=$?
if [ ${status} -eq 1 ];
then
    echo "ERROR: Could not move to scriptLocation specifed."
    exit 1
fi

# Create the line for breaks between playlists
breakText=""
for i in {1..160};
do
    breakText+="-"
done

emailText="From: ${emailSenderUsername}\nTo: ${emailRecipient}\nSubject: Your Weekly YTD video collection update\n\nDownloaded these new videos this week!\n${breakText}"

# For each playlist in the playlist list
for playlist in `cat ${playlistListLocation}`;
do
    # Get the name of the playlist by making a request and pulling the JSON information for the first 
    # video in the playlist
    playlistName=`${youtubeDownloaderLocation} --playlist-end 1 -j -f bestaudio ${playlist} | jq -r .playlist`
    
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
    if [[ ${newlyDownloadedSongs} == "" ]]
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
