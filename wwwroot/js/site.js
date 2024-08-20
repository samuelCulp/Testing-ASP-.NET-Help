
document.addEventListener('DOMContentLoaded', () => {
    // Load JSON data
    loadData();

    // Event listeners for the search buttons
    document.getElementById('search-naming-convention-button').addEventListener('click', handleNamingConventionSearch);
    document.getElementById('search-contact-button').addEventListener('click', handleContactSearch);

    // Event listeners for the buttons that change the table
    const tableButtons = document.querySelectorAll('#button-container button');
    tableButtons.forEach(button => {
        button.addEventListener('click', () => {
            const category = button.getAttribute('data-category');
            const department = button.getAttribute('data-department');
            updateTable(category, department);
        });
    });
});

function loadData() {
    // Load the JSON data for naming conventions
    fetch('/mydata/namingConventions.json', { cache: 'no-store' })// cache no-store stops the browser from saving the table info
        .then(response => response.json())
        .then(jsonData => {
            window.namingConventions = jsonData.namingConventions;
            updateNamingConventionTable('ASKIT');
        })
        .catch(error => {
            console.error('Error loading naming conventions data:', error);
        });

    // Load the JSON data for contact information
    fetch('/mydata/contactInfo.json', { cache: 'no-store' })
        .then(response => response.json())
        .then(jsonData => {
            window.data = jsonData.data;
            updateDepartmentContactTable('ASKITD');
        })
        .catch(error => {
            console.error('Error loading department data:', error);
        });
}

function updateTable(category, department) {
    updateNamingConventionTable(category);
    updateDepartmentContactTable(department);
    displayCurrentTable(category, department);
}

function updateNamingConventionTable(category) {
    const table = document.getElementById('naming-convention-table').querySelector('tbody');
    table.innerHTML = '';

    const names = namingConventions[category] || [];

    const rows = 9;
    const columns = 7;

    let nameIndex = 0;
    for (let i = 0; i < rows; i++) {
        const tr = document.createElement('tr');
        for (let j = 0; j < columns; j++) {
            const td = document.createElement('td');
            td.textContent = names[nameIndex] || '';
            td.classList.remove('highlight'); // Remove previous highlight
            tr.appendChild(td);
            nameIndex++;
        }
        table.appendChild(tr);
    }
}

function updateDepartmentContactTable(department) {
    const tableBody = document.querySelector('#department-contact-table tbody');
    tableBody.innerHTML = '';

    const rows = data[department];
    if (rows) {
        rows.forEach(row => {
            const tr = document.createElement('tr');
            tr.innerHTML = `
                <td>${row.contact}</td>
                <td>${row.email}</td>
                <td>${row.phone}</td>
                <td>${row.group}</td>
            `;
            tableBody.appendChild(tr);
        });
    }
}

function handleNamingConventionSearch() {
    const searchTerm = document.getElementById('search-naming-convention-input').value.toLowerCase().trim();
    let foundMatch = false;

    // Search through naming conventions
    for (let category in namingConventions) {
        const names = namingConventions[category];
        names.forEach((name, index) => {
            if (name.toLowerCase().includes(searchTerm)) {
                showAndHighlight(category, 'naming-convention-table', searchTerm, index);
                const buttonName = getButtonNameByCategory(category);
                displaySearchResult(buttonName, 'Naming Convention');
                updateTableTitle(buttonName, 'Naming Convention');
                foundMatch = true;
            }
        });
    }

    if (!foundMatch) {
        alert('No matches found.');
        document.getElementById('search-result').textContent = '';
        updateTableTitle('', '');
    }
}

function handleContactSearch() {
    const searchTerm = document.getElementById('search-contact-input').value.toLowerCase().trim();
    let foundMatch = false;

    // Search through departments contact information
    for (let department in data) {
        const departmentData = data[department];

        departmentData.forEach(row => {
            if (
                row.contact.toLowerCase().includes(searchTerm) ||
                row.email.toLowerCase().includes(searchTerm) ||
                row.phone.toLowerCase().includes(searchTerm) ||
                row.group.toLowerCase().includes(searchTerm)
            ) {
                showAndHighlight(department, 'contact-table', searchTerm);
                const buttonName = getButtonNameByDepartment(department);
                displaySearchResult(buttonName, 'Contact Information');
                updateTableTitle(buttonName, 'Contact Information');
                foundMatch = true;
            }
        });
    }

    if (!foundMatch) {
        alert('No matches found.');
        document.getElementById('search-result').textContent = '';
        updateTableTitle('', '');
    }
}

function showAndHighlight(categoryOrDepartment, tableType, searchTerm, nameIndex = null) {
    updateTable(categoryOrDepartment, categoryOrDepartment);

    setTimeout(() => {
        if (tableType === 'naming-convention-table' && nameIndex !== null) {
            highlightNamingConventionCell(nameIndex, searchTerm);
        } else if (tableType === 'contact-table') {
            highlightContactTableCell(searchTerm);
        }
    }, 100);
}

function highlightNamingConventionCell(nameIndex, searchTerm) {
    const table = document.getElementById('naming-convention-table').querySelector('tbody');
    const cells = Array.from(table.getElementsByTagName('td'));

    removeHighlights();

    const cellToHighlight = cells[nameIndex];
    if (cellToHighlight) {
        cellToHighlight.classList.add('highlight');
        highlightText(cellToHighlight, searchTerm);
    }
}

function highlightContactTableCell(searchTerm) {
    const table = document.getElementById('department-contact-table').querySelector('tbody');
    const rows = table.getElementsByTagName('tr');

    for (let row of rows) {
        Array.from(row.getElementsByTagName('td')).forEach(td => {
            if (td.textContent.toLowerCase().includes(searchTerm)) {
                td.classList.add('highlight');
                highlightText(td, searchTerm);
            }
        });
    }
}

function highlightText(element, searchTerm) {
    const regex = new RegExp(`(${searchTerm})`, 'gi');
    element.innerHTML = element.textContent.replace(regex, '<span class="highlight-text">$1</span>');
}

function removeHighlights() {
    const highlightedCells = document.querySelectorAll('#naming-convention-table .highlight, #department-contact-table .highlight');
    highlightedCells.forEach(cell => {
        cell.classList.remove('highlight');
        cell.innerHTML = cell.textContent; // Remove any added HTML for highlighting
    });
}
function handleNamingConventionSearch() {
    const searchTerm = document.getElementById('search-naming-convention-input').value.toLowerCase().trim();
    const terms = searchTerm.split(' ').filter(term => term.length > 0); // Split by spaces and filter out empty terms

    if (terms.length === 0) {
        alert('Please enter a search term.');
        return;
    }

    let foundMatch = false;

    for (let category in namingConventions) {
        const names = namingConventions[category];
        names.forEach((name, index) => {
            const nameLower = name.toLowerCase();
            if (terms.every(term => nameLower.includes(term))) {
                showAndHighlight(category, 'naming-convention-table', searchTerm, index);
                const buttonName = getButtonNameByCategory(category);
                displaySearchResult(buttonName, 'Naming Convention');
                updateTableTitle(buttonName, 'Naming Convention');
                foundMatch = true;
            }
        });
    }

    if (!foundMatch) {
        alert('No exact matches found.');
        document.getElementById('search-result').textContent = '';
        updateTableTitle('', '');
    }
}

function showAndHighlight(categoryOrDepartment, tableType, searchTerm, nameIndex = null) {
    updateTable(categoryOrDepartment, categoryOrDepartment);

    setTimeout(() => {
        if (tableType === 'naming-convention-table' && nameIndex !== null) {
            highlightNamingConventionCell(nameIndex, searchTerm);
        } else if (tableType === 'contact-table') {
            highlightContactTableCell(searchTerm);
        }
    }, 100);
}

function highlightNamingConventionCell(nameIndex, searchTerm) {
    const table = document.getElementById('naming-convention-table').querySelector('tbody');
    const cells = Array.from(table.getElementsByTagName('td'));

    removeHighlights();

    const cellToHighlight = cells[nameIndex];
    if (cellToHighlight) {
        cellToHighlight.classList.add('highlight');
        highlightText(cellToHighlight, searchTerm);
    }
}

function highlightText(element, searchTerm) {
    // Highlight text based on the search term
    const regex = new RegExp(`(${searchTerm})`, 'gi');
    element.innerHTML = element.textContent.replace(regex, '<span class="highlight-text">$1</span>');
}

function removeHighlights() {
    const highlightedCells = document.querySelectorAll('#naming-convention-table .highlight, #department-contact-table .highlight');
    highlightedCells.forEach(cell => {
        cell.classList.remove('highlight');
        cell.innerHTML = cell.textContent; // Remove any added HTML for highlighting
    });
}


function displaySearchResult(buttonName, tableType) {
    const resultElement = document.getElementById('search-result');
    resultElement.textContent = `Search Result: Found in ${tableType} for ${buttonName}`;
}

function getButtonNameByDepartment(department) {
    const departmentMap = {
        'ASKITD': 'Central IT (ASK IT)',
        'SURGD': 'HSIS - Formerly Dept of Surg',
        '1917D': '1917 IT Support RISC',
    };
    return departmentMap[department] || department;
}

function getButtonNameByCategory(category) {
    const categoryMap = {
        'ASKIT': 'Central IT (ASK IT)',
        'Surg': 'HSIS - Formerly Dept of Surg',
        '1917': '1917 IT Support RISC',
    };
    return categoryMap[category] || category;
}

function updateTableTitle(buttonName, tableType) {
    const titleElement = document.getElementById('current-table-name');
    titleElement.textContent = `Current Table: ${buttonName} - ${tableType}`;
}

function displayCurrentTable(category, department) {
    const resultElement = document.getElementById('search-result');
    resultElement.textContent = `Displaying: Naming Convention - ${category} | Department Contacts - ${department}`;
}


//https://en.wikipedia.org/wiki/Rickrolling