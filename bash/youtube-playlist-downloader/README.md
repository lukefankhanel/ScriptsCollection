# Youtube Playlist Downloader
A script that uses the Youtube-dl (Youtube Downloader) command line interface to download music files from a list of specified Youtube Music Playlists. Users can put their music playlists in a file, which will then be read and each playlist's videos will be downloaded into their own directory. The script supports the downloading of only new videos from a particular playlist. After all new videos have been downloaded, the user is notified via email. This script is meant to be run by cronjob. For example, once per week, so that any newly added videos will be downloaded and the user can be notified of the downloads through email.

# Requirements
1. youtube-dl
2. ffmpeg

# Usage
1. Add your list of playlists to a text file called "playlists" with each playlist separated by a newline, make sure the playlists are publicly available, or unlisted
2. Set up a cronjob to run the script to download new videos in your desired timeframe; have this cronjob point to the script
3. Add your desired SMTP sever and user credentials for the email functionality, explained below
```
SMTPServer                  -- The SMTP Server URL to use
emailRecipient              -- The email that will recieve the notification email from the script
emailSenderUsername=        -- The username of the email that sends the notification email
emailSenderPassword         -- The password of the email that sends the notification email
scriptLocation              -- The fully qualified location of the script
playlistListLocation        -- The location of the text list of playlists
youtubeDownloaderLocation   -- The fully qualified location of youtube-dl (run which youtube-dl)
```
