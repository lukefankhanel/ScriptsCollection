# FolderCreator

A simple command line Python application to create a template directory structure for each course in a school semester based on a given list of course names. A folder for each course for a semester will be generated along with folders for each week of the course based on the start date and end date of the semester.

# Instructions
1. You can run the FolderCreator script from anywhere, the desired Folders will be created in your current working directory.
2. When run, you can enter your list of courses to create Week directories for.
3. Then you can enter the start and end dates of a semester by following the format shown. Each date in a week will be converted to the last monday that took place. So, if you enter January 9th 2021 as your start date, your semester's first week will start on the 4th.
4. Lastly, you can add a list of weeks to ignore from the semester by entering any day on that week for each week to be ignored. So, if your semester started from the 9th of January 2021 and ended on the 29th, you could also enter 13/1/2020 to ignore the week of January the 11th to make your semester 3 weeks long.
5. Each course will have its directories created along with an Extra folder for notes and Zoom information.
