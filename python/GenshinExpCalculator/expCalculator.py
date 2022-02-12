#6 blue 5'000
#4 purple == 20'000

#Minimum is 110'000, 6, 4
#Average is 122'500, 6.5 blue, 4.5 purple
#Maximum is 135'000, 7, 5

#Total exp needed 8'362'650
#Total Mora needed 1'673'400

import sys

minimumRuns = [135000, 7, 5]
averageRuns = [122500, 6.5, 4.5]
maximumRuns = [110000, 6, 4]

levelMappings = [[1,0],[20,120175],[40,698500],[50,1277600],[60,2131725],[70,3327650],[80,4939525],[90,8362650]]

def addRun(tupleValues, tupleTotals):
    for i in range(len(tupleTotals)):
        if i != len(tupleTotals) - 1:
            tupleTotals[i] += tupleValues[i]
        else:
            tupleTotals[i] += 1
    return tupleTotals

def findLevelMapping(level):
    if level > 90:
        return (levelMappings[len(levelMappings) - 1])[1]
    for i in range(len(levelMappings)):
        if level < (levelMappings[i + 1])[0]:
            return (levelMappings[i])[1]
        elif level == (levelMappings[i + 1])[0]:
            return(levelMappings[i + 1])[1]
    

def calculateExpNeeded(currentCharacterLevel, desiredLevel):
    totalMinimumRuns = [0,0,0,0]
    totalAverageRuns = [0,0,0,0]
    totalMaximumRuns = [0,0,0,0]

    characterCurrentExp = findLevelMapping(currentCharacterLevel)
    characterDesiredExp = findLevelMapping(desiredLevel)
    expCalculation = characterDesiredExp - characterCurrentExp

    while (totalMaximumRuns[0] < expCalculation):
        if (totalMinimumRuns[0] < expCalculation):
            totalMinimumRuns = addRun(minimumRuns, totalMinimumRuns)
        if (totalAverageRuns[0] < expCalculation):
             totalAverageRuns = addRun(averageRuns, totalAverageRuns)
        totalMaximumRuns = addRun(maximumRuns, totalMaximumRuns)

    return [totalMinimumRuns, totalAverageRuns, totalMaximumRuns]

def printValues(totalBlueBooks, totalPurpleBooks, totalRuns):
    print("Total blue books aquired: " + str(totalBlueBooks))
    print("Total purple books aquired: " + str(totalPurpleBooks))
    print("After: " + str(totalRuns) + " runs.")
    print("--------------")

def main(levels):
    values = calculateExpNeeded(int(levels[1]), int(levels[2]))
    print("With the lowest possible drop rate:")
    printValues((values[2])[1], (values[2])[2], (values[2])[3])
    print("With an average drop rate:")
    printValues((values[1])[1], (values[1])[2], (values[1])[3])
    print("With the highest possible drop rate:")
    printValues((values[0])[1], (values[0])[2], (values[0])[3])

if __name__ == "__main__":
    if len(sys.argv) != 3:
        print(sys.argv)
        print("Usage: <Character Current Level> <Desired Character Level>")
    else:
        main(sys.argv)