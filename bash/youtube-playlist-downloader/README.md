# Youtube Playlist Downloader
A script that uses the [Youtube-dl](https://github.com/ytdl-org/youtube-dl) (Alternatively, [yt-dlp](https://github.com/yt-dlp/yt-dlp)) command line interface tool to download audio files from a list of specified normal YouTube playlists or channel playlists. Users can put their playlist links in a file, which will then be read, and each playlist's videos will be downloaded into their own directory. The script supports the downloading of only new videos from a particular playlist. After all new videos have been downloaded, the user is notified via email. This script is meant to be run by cronjob. For example, once per week, so that any newly added videos will be downloaded and the user can be notified of the downloads through email.

# Requirements
1. Bash
2. youtube-dl
3. ffmpeg
4. An API key from a project created within the [Google Developers Console](https://console.developers.google.com/) that has access enabled to the YouTube Data API v3.

# Usage
1. Add your list of playlists to a text file called "playlists" with each playlist separated by a newline, make sure the playlists are publicly available, or unlisted 
2. Add your desired SMTP sever, the user credentials for the email functionality, and all other credentials, described below. NOTE: For sending emails through a GMail account, you'll have to set up an [App Password](https://support.google.com/accounts/answer/185833?hl=en)
3. Set up a cronjob to run the script to download new videos in your desired timeframe; have this cronjob point to the script
```
googleAPIKey                -- The API key for use with Google's YouTube Data API.
SMTPServer                  -- The SMTP Server URL to use
emailRecipient              -- The email that will recieve the notification email from the script
emailSenderUsername=        -- The username of the email that sends the notification email
emailSenderPassword         -- The password of the email that sends the notification email
scriptLocation              -- The fully qualified location of the script
playlistListLocation        -- The location of the text list of playlists
youtubeDownloaderLocation   -- The fully qualified location of youtube-dl (run which youtube-dl)
```

# Troubleshooting
+ Log files are generated under each playlist's sub-directory, and there you can see the full output of the download command that was run, with each run separated by date.

+ When something goes wrong, look for the "error-log.txt" file that is output into the "scriptLocation" you've specified above. Remember that it's impossible to download premier videos!
