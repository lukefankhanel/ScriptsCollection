#!/bin/bash

if [ $# != 2 ]
then
    echo "Usage: download-single-video.sh <Video to download> <audio|video>"
    exit 1
fi

if [[ $2 != "audio" ]] && [[ $2 != "video" ]]
then
    echo "dsf"
fi

formatString=""
if [[ $2 == "audio" ]]
then
    formatString+="-x -f bestaudio"
fi

youtube-dl \
    -o './output/%(title)s - %(id)s.%(ext)s' \
    --add-metadata ${formatString} ${1}

echo "Finished downloading. Stored output in /output"
