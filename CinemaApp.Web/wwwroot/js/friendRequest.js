document.addEventListener('DOMContentLoaded', function () {
    const becomeFriendsButton = document.getElementById('becomeFriend');

    if (becomeFriendsButton) {
        becomeFriendsButton.addEventListener('click', function () {
            const query = becomeFriendsButton.getAttribute('data-username');

            fetch('https://localhost:7015/api/FriendRequestApi/SendFriendRequest', {
                method: 'POST',  // Change to POST for sending data in the body
                credentials: 'include',  // Ensure cookies are sent if authentication is required
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'  // Set Content-Type for form data
                },
                body: new URLSearchParams({
                    'username': query  // Send the 'username' as form data
                })
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error("Failed to send friend request.");
                    }

                    alert('The friend request sent successfully!');
                })
                .catch(error => {
                    console.error('Error:', error);
                    alert('An error occurred while sending the friend request.');
                });
        });
    } else {
        console.error('Button with id "becomeFriend" not found!');
    }
});