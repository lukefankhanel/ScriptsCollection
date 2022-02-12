# Youtube Playlist Downloader
A script that uses the Youtube-dl (Youtube Downloader) command line interface to download music files from a list of specified Youtube Music Playlists. Users can put their music playlists in a file, which will then be read and each playlist's videos will be downloaded into their own directory. The script supports the downloading of only new videos from a particular playlist. After all new videos have been downloaded, the user is notified via email. This script is meant to be run by cronjob. For example, once per week, so that any newly added videos will be downloaded and the user can be notified of the downloads through email.

# Usage
1. Add your list of playlists to the "playlists" file with each playlist separated by a newline
2. Set up a cronjob to run the script to download new videos in your desired timeframe; have this cronjob point to the script
3. Add your desired SMTP sever and user credentials for the email functionality