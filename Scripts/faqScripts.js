window.onload = function () {
    "use strict";
    var container = document.getElementById("category-container");
    var allRows = document.getElementsByClassName("question");

    //Adding an event listener to the category container
    container.addEventListener("click", (event) => {
        //Defines what a button is
        const isButton = event.target.nodeName === "BUTTON";
        //Checks to see if a button is clicked
        if (!isButton) {
            //Returns if a button is not clicked
            return;
        } else {
            //Entering here means a button was clicked
            //Functionality starts here

            //Checks if the button that was clicked has the "selected-btn" class
            if (event.target.classList.contains('selected-btn')) {
                //Removes "selected-btn" class
                event.target.classList.remove('selected-btn');
                //Calls function which shows the rows that have the category
                showRows(event.target.innerHTML);
                //Call function here
            } else {
                //Adds "selected-btn" class
                event.target.classList.add('selected-btn');
                //Calls function which hides rows that have the category
                hideRows(event.target.innerHTML);
                //Call funcion here
            }
        }
    })

    function hideRows(category) {
        for (var i = 0; i < allRows.length; i++) {
            if (!allRows[i].classList.contains(category)) {
                allRows[i].classList.add('hidden-faq');

            }
        }
    }

    function showRows(category) {
        for (var i = 0; i < allRows.length; i++) {
            if (!allRows[i].classList.contains(category)) {
                allRows[i].classList.remove('hidden-faq');
            }
        }
    }
}