
var colors = ["#ff6d6d", "#ffb672", "#fffa71", "#8fff93", "#81e8ff", "#ba52ff", "#ff69f7"];
//var colors = ["red", "orange", "yellow", "green", "blue", "indigo", "violet"];

var index = 0;
// function showValue(newValue) {
//     document.getElementById("range").innerHTML = newValue;
// }

function changeColor() {
    var button = document.querySelector("button");
    button.style.backgroundColor = colors[index];
    var title = document.querySelector("h1");
    title.style.color = colors[index];
    index = (index + 1) % colors.length;
}

function generatePassword() {
    var password = "";
    var numberField = document.querySelector("input[type='number']");
    var passwordLength = numberField.value;
    for (var i = 0; i < passwordLength; i++) {
        var generatedCharacter;

        if (password.length >= 1) {
            generatedCharacter = generateCharacter();
            while (generatedCharacter == password.charAt(password.length - 1)) {
                generatedCharacter = generateCharacter();
            }
        } else {
            generatedCharacter = generateCharacter(true);
        }
        password += generatedCharacter
    }
    var textField = document.querySelector("input[type='text']");
    textField.value = password;
}

function generateCharacter(firstCharacter) {
    if (firstCharacter === undefined) {
        firstCharacter = false;
    }
    var characters;

    //All character string, use this once everything is moved over to Excel
    //Be careful of backslashes though...
    //ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+-=[]{}|;':\"<>,.?/\\
    if (firstCharacter) {
        characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    } else {
        characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789@#^&*)_=];':\"<>,.";
    }
    return characters.charAt(Math.floor(Math.random() * characters.length));
}
setInterval(changeColor, 2500);
var textField = document.querySelector("input[type='text']");
textField.addEventListener("click", function () {
    this.select();
});
