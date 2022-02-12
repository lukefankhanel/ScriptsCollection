
#!THE PROBLEM!
#An isogram is a word that has no repeating letters, 
# consecutive or non-consecutive. Implement a function that determines whether 
# a string that contains only letters is an isogram. Assume the empty string is an isogram. 
# Ignore letter case.





#!BEST SOLUTION!
# def is_isogram(string):
#     return len(string) == len(set(string.lower()))

#Since set() returns just one instance of each character regardless of how many times it repeats, 
# you can check the length of the whole string versus how many individual characters there is.
#print(set("abbsaacc"))

def is_isogram(string):
    if string.isalpha() or string == "":
        string = string.upper()
        for x in range(len(string)):
            #print(str(x) + " " + string)
            if string[x + 1 : len(string)].find(string[x]) != -1:
                return False
    else:
        return False
            
    return True


def main():
    # if "".isalpha():
    #     print("True")
    # else:
    #     print("False")
    #print("asd"[1:5])
    #print("".find("a"))
    #is_isogram("asd")

    print("Running Tests")
    assert is_isogram("isogram") == True
    assert is_isogram("as39") == False
    assert is_isogram("Dermatoglyphics") == True
    assert is_isogram("aba") == False, "same chars may not be adjacent"
    assert is_isogram("moOse")  == False, "same chars may not be same case" 
    assert is_isogram("isIsogram") == False 
    assert is_isogram("") == True, "an empty string is a valid isogram" 
    print("Tests Completed")

if __name__ == "__main__":
    main()
    