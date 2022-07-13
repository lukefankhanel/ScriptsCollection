#!/bin/bash

# Sends an email using the Gmail API and OAuth. 
# In order to use, a user must enter in the following URL string into their browser, copy the code into the curl command, 
# then retreive the refresh token from the request, and only then, can this script be used.
# Note: given that the Service is only a Google Testing Service, the refresh token is only valid for 7 days 
# unlike normal refresh tokens that are generally valid indefinitely.
# https://stackoverflow.com/questions/8953983/do-google-refresh-tokens-expire

#Put this URL into your browser to get a one-time code for your first curl request.
#https://accounts.google.com/o/oauth2/v2/auth?client_id=[oAuthClientID HERE]&redirect_uri=urn:ietf:wg:oauth:2.0:oob&scope=https://mail.google.com/&response_type=code

#Put your code from the last step in the [CODE HERE] section, then run the command.
#curl \
#--request POST \
#--data "code=[CODE HERE]&client_id=[oAuthClientID HERE]&client_secret=[oAuthClientSecret HERE]&redirect_uri=urn:ietf:wg:oauth:2.0:oob&grant_type=authorization_code" \
#https://accounts.google.com/o/oauth2/token


#set -x

usageString="Usage:
\nSyntax: ./sendEmail.sh -[r|s|b] -{h}
\nOptions:
\n-r    emailRecipient -- The person who will receive the email.
\n-s    emailSubject -- The email's subject.
\n-b    emailBody -- The body text of the email.
\n-h    Help -- This message."

emailSender=""
emailRecipient=""
emailSubject=""
emailBody=""
oAuthClientID=""
oAuthClientSecret=""
oAuthAccessToken=""
oAuthRefreshToken=""

# Get the options from the command and put them into their corresponding variables
while getopts "r:s:b:h" option; do
   case ${option} in
        r)
            emailRecipient=${OPTARG}
            ;;
        s)
            emailSubject=${OPTARG}
            ;;
        b)
            emailBody=${OPTARG}
            ;;
        h)
            echo -e ${usageString}
            exit 0
            ;;
   esac
done

if [ -z "$emailRecipient" ] || [ -z "$emailSubject" ] || [ -z "$emailBody" ]; then
    echo -e ${usageString}
    echo 'ERROR: Missing email recipient, subject, or body.'
    exit 1
fi


# Build the raw email text string.
rawEmailText="From: ${emailSender}\nTo: ${emailRecipient}\nSubject: ${emailSubject}\n\n${emailBody}"

# Take the email text string and convert it into base 64
# Note the -e on echo. This is needed to evaluate the \n's correctly
emailText=$(echo -e ${rawEmailText} | base64 -w 0)


#Request for a new access token based on the Refresh Token
oAuthAccessToken=$(curl --request POST \
--url "https://accounts.google.com/o/oauth2/token" \
--data "client_id=${oAuthClientID}&client_secret=${oAuthClientSecret}&refresh_token=${oAuthRefreshToken}&grant_type=refresh_token" \
| jq .access_token -r)


#The URL from the documentation -- Couldn't get this one to work
#https://gmail.googleapis.com/upload/gmail/v1/users/[EMAIL HERE]/messages/send

# Send the email by a curl request
curl --silent --request POST \
    --url "https://gmail.googleapis.com/gmail/v1/users/${sender}/messages/send?alt=json" \
    -H "Content-Type: application/json" \
    -H "Accept: application/json" \
    -H "Authorization: Bearer ${oAuthAccessToken}" \
    --data "{\"raw\": \"${emailText}\"}"
