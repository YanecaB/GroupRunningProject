// Get references to modal elements
const searchButton = document.getElementById('searchButton');
const searchModal = document.getElementById('searchModal');
const closeSearch = document.getElementById('closeSearch');
const searchContent = document.getElementById('searchContent');

// Prevent navigation for the Search button
document.getElementById('searchButton').addEventListener('click', function (event) {
    event.preventDefault(); // Prevent default link behavior
    document.getElementById('searchModal').style.display = 'block';
});


// Open the search modal
searchButton.addEventListener('click', function () {
    // Show the modal
    searchModal.style.display = 'block';

    // Dynamically load the partial view content into the modal
    fetch('/Shared/_SearchPartial') // Replace with your controller/action if different
        .then(response => response.text())
        .then(html => {
            searchContent.innerHTML = html;

            // Add event listener to the search input after loading the content
            const searchInput = document.getElementById('searchInput');
            searchInput.addEventListener('input', function () {
                const query = searchInput.value;

                if (query.length > 2) { // Start searching after 3 characters
                    fetch(`https://localhost:7018/SearchApi/SearchUsers?username=${encodeURIComponent(query)}`)
                        .then(response => response.json())
                        .then(data => {
                            const resultsContainer = document.getElementById('searchResults');
                            resultsContainer.innerHTML = ''; // Clear previous results

                            if (data.length > 0) {
                                data.forEach(user => {
                                    const resultItem = document.createElement('div');
                                    resultItem.className = 'search-result-item';
                                    resultItem.innerHTML = `
                                        <strong>${user.username}</strong>
                                        <img src="${user.profilePicturePath || '/default-avatar.jpg'}" 
                                             alt="Profile Picture" width="40" height="40" style="margin-left: 10px;" />
                                    `;
                                    resultsContainer.appendChild(resultItem);
                                });
                            } else {
                                resultsContainer.innerHTML = '<p>No users found.</p>';
                            }
                        });
                }
            });
        });
});

// Close the modal
closeSearch.addEventListener('click', function () {
    searchModal.style.display = 'none';
});

// Close the modal when clicking outside the modal content
window.addEventListener('click', function (event) {
    if (event.target === searchModal) {
        searchModal.style.display = 'none';
    }
});
