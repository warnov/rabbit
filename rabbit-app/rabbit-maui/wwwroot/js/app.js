window.focusLastKmInput = () => {
    // Find all km input fields
    const kmInputs = document.querySelectorAll('.km-input');
    if (kmInputs.length > 0) {
        // Focus the last one
        kmInputs[kmInputs.length - 1].focus();
    }
};

window.selectAllText = (element) => {
    if (element) {
        element.select();
    }
};
