// State selection for dropdown functionality
function populateStateDropdown() {
    const stateDropdowns = document.querySelectorAll('.usa-states-dropdown');
    
    // List of US states with abbreviations
    const states = [
        { name: "Alabama", abbr: "AL" },
        { name: "Alaska", abbr: "AK" },
        { name: "Arizona", abbr: "AZ" },
        { name: "Arkansas", abbr: "AR" },
        { name: "California", abbr: "CA" },
        { name: "Colorado", abbr: "CO" },
        { name: "Connecticut", abbr: "CT" },
        { name: "Delaware", abbr: "DE" },
        { name: "Florida", abbr: "FL" },
        { name: "Georgia", abbr: "GA" },
        { name: "Hawaii", abbr: "HI" },
        { name: "Idaho", abbr: "ID" },
        { name: "Illinois", abbr: "IL" },
        { name: "Indiana", abbr: "IN" },
        { name: "Iowa", abbr: "IA" },
        { name: "Kansas", abbr: "KS" },
        { name: "Kentucky", abbr: "KY" },
        { name: "Louisiana", abbr: "LA" },
        { name: "Maine", abbr: "ME" },
        { name: "Maryland", abbr: "MD" },
        { name: "Massachusetts", abbr: "MA" },
        { name: "Michigan", abbr: "MI" },
        { name: "Minnesota", abbr: "MN" },
        { name: "Mississippi", abbr: "MS" },
        { name: "Missouri", abbr: "MO" },
        { name: "Montana", abbr: "MT" },
        { name: "Nebraska", abbr: "NE" },
        { name: "Nevada", abbr: "NV" },
        { name: "New Hampshire", abbr: "NH" },
        { name: "New Jersey", abbr: "NJ" },
        { name: "New Mexico", abbr: "NM" },
        { name: "New York", abbr: "NY" },
        { name: "North Carolina", abbr: "NC" },
        { name: "North Dakota", abbr: "ND" },
        { name: "Ohio", abbr: "OH" },
        { name: "Oklahoma", abbr: "OK" },
        { name: "Oregon", abbr: "OR" },
        { name: "Pennsylvania", abbr: "PA" },
        { name: "Rhode Island", abbr: "RI" },
        { name: "South Carolina", abbr: "SC" },
        { name: "South Dakota", abbr: "SD" },
        { name: "Tennessee", abbr: "TN" },
        { name: "Texas", abbr: "TX" },
        { name: "Utah", abbr: "UT" },
        { name: "Vermont", abbr: "VT" },
        { name: "Virginia", abbr: "VA" },
        { name: "Washington", abbr: "WA" },
        { name: "West Virginia", abbr: "WV" },
        { name: "Wisconsin", abbr: "WI" },
        { name: "Wyoming", abbr: "WY" },
        { name: "District of Columbia", abbr: "DC" }
    ];
    
    stateDropdowns.forEach(dropdown => {
        // Skip if the dropdown is already populated
        if (dropdown.options.length > 1) {
            return;
        }
        
        // Get currently selected value (if any)
        const currentValue = dropdown.value;
        
        // Clear existing options (except placeholder)
        while (dropdown.options.length > 1) {
            dropdown.remove(1);
        }
        
        // Add states to the dropdown
        states.forEach(state => {
            const option = document.createElement('option');
            option.value = state.abbr;
            option.text = `${state.name} (${state.abbr})`;
            dropdown.add(option);
            
            // Select the option if it matches the current value
            if (currentValue === state.abbr) {
                option.selected = true;
            }
        });
    });
}

// Initialize the state dropdowns on page load
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', populateStateDropdown);
} else {
    // DOMContentLoaded has already fired
    populateStateDropdown();
}