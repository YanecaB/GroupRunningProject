document.addEventListener('DOMContentLoaded', function () {
    const searchButton = document.getElementById('searchButton');
    const searchModal = document.getElementById('searchModal');
    const closeSearch = document.getElementById('closeSearch');
    const searchContent = document.getElementById('searchContent');
    const searchInput = document.getElementById('searchInput');

    // Open the search modal
    searchButton.addEventListener('click', function (event) {
        event.preventDefault();
        searchModal.style.display = 'block';  // Show the modal
    });

    // Close the search modal
    closeSearch.addEventListener('click', function () {
       searchModal.style.display = 'none';  // Hide the modal
    });

    // Close modal when clicking outside
    window.addEventListener('click', function (event) {
        if (event.target === searchModal) {
            searchModal.style.display = 'none';
        }
    });

    // Handle search input event
    searchInput.addEventListener('input', function () {
        const query = searchInput.value;

        if (query.length > 0) {  // Start searching after 3 characters
            fetch(`https://localhost:7018/api/SearchApi/SearchUsers?username=${encodeURIComponent(query)}`)
                .then(response => {
                    if (!response.ok) {
                        throw new Error("Failed to load search results.");
                    }
                    return response.json();
                })
                .then(data => {
                    const resultsContainer = document.getElementById('searchResults');
                    resultsContainer.innerHTML = '';  // Clear previous results

                    if (data.length > 0) {
                        data.forEach(user => {
                            const resultItem = document.createElement('div');
                            resultItem.className = 'search-result-item';
                            resultItem.innerHTML = `
                                <strong>${user.username}</strong>
                                <img src="${user.profilePicturePath}" 
                                     alt="Profile Picture" style="width: 30px; height: 30px; border-radius: 50%; object-fit: cover; border: 0.5px solid #ddd; margin-left:3%;" />
                            `;
                            resultsContainer.appendChild(resultItem);
                        });
                    } else {
                        resultsContainer.innerHTML = '<p>No users found.</p>';
                    }
                })
                .catch(error => {
                    console.error("Error loading search results:", error);
                });
        }
    });
});
