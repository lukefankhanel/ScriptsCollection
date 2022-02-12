import os
import datetime


# Main function to get user input and instantiate objects.
def main():
    dateDelimiter = "-"
    courseString = input("What is the list of courses? (Separated by a \",\" NO SPACES AFTER COMMAS!):")
    startString = input("What is the semester start date? (Of the format: dd/mm/yyyy):")
    endString = input("What is the semester end date? (Of the format: dd/mm/yyyy):")
    breakWeeks = input("Are there any break weeks in the semester? "
    + "(Of the format: dd/mm/year, dd/mm/yyyy, etc. - Press enter if none):")

    courseList = courseString.split(",")
    breakList = breakWeeks.split(",")
    
    c = CalendarCalculator(startString, endString, breakList, dateDelimiter)
    f = FolderCreator(os.getcwd(), courseList, c.calculateWeekList())
    
    print("Creating Directories...")
    f.createDirectoryStructure()
    print("Directories Created!")


# Class to take date strings from the user and calculate and return which Monday's in a semester are valid.
class CalendarCalculator:
    # Initalizes the CalendarCalculator and converts the calendar date strings received into workable python date objects
    # semesterStartDate - A string representing the start date of the semester in the format dd/mm/yyyy
    # semesterEndDate - A string representing the start date of the semester in the format dd/mm/yyyy
    # breakWeeks - A list of date strings representing the weeks to be ignored when calculating the weeks in a semester
    # dateDelimiter - The string that determines what character the calculated dates will be delimied by when output
    def __init__(self, semesterStartDate, semesterEndDate, breakWeeks, dateDelimiter):
        self.semesterStartDate = self.findMostRecentMonday(self.convertToDateObject(semesterStartDate))
        self.semesterEndDate = self.findMostRecentMonday(self.convertToDateObject(semesterEndDate))
        self.dateDelimiter = dateDelimiter

        if breakWeeks[0] == "":
            self.breakWeeks = []
        else:    
            self.breakWeeks = self.convertDateWeeks(breakWeeks)
        
    # Converts a string into a proper Python date object
    # stringValue - The string to convert
    # Return a Python date object based on the passed in string.
    def convertToDateObject(self, stringValue):
        splitString = stringValue.split("/")
        return datetime.date(int(splitString[2]), int(splitString[1]), int(splitString[0]))

    # Finds the most recent monday from the given date object and sets the date objects time to that found monday.
    # date - The date to find its relative monday
    # Return A date object with its date set to the most recent monday.
    def findMostRecentMonday(self, date):
        mondayDifference = date.weekday()
        return date + datetime.timedelta(days=-mondayDifference)

    # Converts a list of date strings into Python date objects and finds their relative mondays
    # stringList - The list of date strings to convert
    # Return A list of Python date objects set to their relative monday.
    def convertDateWeeks(self, stringList):
        breakList = []
        for week in stringList:
            breakList.append(self.findMostRecentMonday(self.convertToDateObject(week)))
        return breakList

    # Calculates the list of weeks in a semester while ignoring any break weeks.
    # Return A list of weeks in the given time frame.
    def calculateWeekList(self):
        currentSemesterDate = self.semesterStartDate
        oneWeek = datetime.timedelta(days=7)
        weekList = []

        while(currentSemesterDate <= self.semesterEndDate):
            if currentSemesterDate not in self.breakWeeks:
                weekList.append(currentSemesterDate.strftime("%d" + self.dateDelimiter + "%b" + self.dateDelimiter + "%Y"))
            currentSemesterDate += oneWeek
        return weekList


# Class that creates the directory structures based on the calculated list of Weeks for each Class in the semester.
class FolderCreator:
    # Initalizes the folder creator.
    # targetDir - The root directory where the folders will be created.
    # courseList - The list of strings representing the courses to create semester folders for.
    # weekList - The list of weeks in a semester to create week folders for.
    def __init__(self, targetDir, courseList, weekList):
        self.targetDir = targetDir
        self.courseList = courseList
        self.weekList = weekList

    # Creates the directory structure based on the targetDir. A folder is generated for
    # each class, and each class is generated a list of folders for each week in the semester.
    # An Extra folder is generated for each class with a Notes.txt file and a place 
    # to store Zoom class information for the class.
    def createDirectoryStructure(self):
        for name in self.courseList:
            courseDir = self.targetDir + "\\" + name
            extraDir = courseDir + "\\" + "Extra"

            try:
                os.mkdir(courseDir)
                weekCount = 1
                for week in self.weekList:
                    folderName = "Week " + str(weekCount) + " (" + week + ")"
                    os.mkdir(courseDir + "\\" + folderName)
                    weekCount += 1
                
                os.mkdir(extraDir)
                open(extraDir + "\\Notes.txt", "w")
                open(extraDir + "\\Zoom.txt", "w")
            except Exception as e:
                print("Error: " + e)
            

if __name__ == "__main__":
    main()