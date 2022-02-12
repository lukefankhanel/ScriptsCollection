import smtplib, ssl, subprocess

#Main method
def main():
    normalSujectText = "Your Weekly RAID Status Update"
    importantSubjectText = "EMERGENECY!!! The RAID Array is not in perfect condition!"

    message = getConsoleOutput()
    if checkRAIDStatus(message) == 0:
        sendMail(normalSujectText, message)
    else:
        sendMail(importantSubjectText, message)
   
#Gets the output of the commands to run in the commands variable and concatinates them into a string which is returned
def getConsoleOutput():
    commands = ["sudo mdadm --detail /dev/md127", "cat /proc/mdstat"]

    finalOutput = ""
    for command in commands:
        argumentList = command.split(" ")
        commandResult = subprocess.run(argumentList, stdout=subprocess.PIPE)
        finalOutput = finalOutput + "--[OUTPUT OF: " + command + "]--\n" + commandResult.stdout.decode("utf-8")
        if command != commands[-1]:
            finalOutput = finalOutput + "\n\n"
    return finalOutput

#Checks to see if the RAID array is still running at full capacity
def checkRAIDStatus(text):
    if text.find("[UU]") == -1:
        return 1
    return 0

#Sends an email using SMTP
def sendMail(subject, messageBody):
    smtpServer = "smtp.gmail.com"
    smtpPort = 587

    emailSender = ""
    emailSenderPass = ""
    emailReceiver = ""

    context = ssl.create_default_context()

    mailMessage = "Subject: " + subject + "\n\n" + messageBody

    print("Opening SMTP Connection...")
    with smtplib.SMTP(smtpServer, smtpPort) as connection:
        connection.starttls(context=context)
        connection.login(emailSender, emailSenderPass)

        print("Now sending mail...")
        connection.sendmail(from_addr=emailSender, to_addrs=emailReceiver, msg=mailMessage)
        print("Mail Sent. Closing Connection.")
    print("Connection Closed. Ending.")


if __name__ == "__main__":
    main()