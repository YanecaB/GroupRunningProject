document.addEventListener('DOMContentLoaded', function () {
    const confirmButton = document.getElementById('confirm');

    if (confirmButton) {
        confirmButton.addEventListener('click', function () {
            const senderUsername = confirmButton.getAttribute('data-username');
            const currentUserUsername = window.currentUsername;

            fetch(`https://localhost:7018/api/FriendRequestApi/ConfirmRequest`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    currentUserUsername: currentUserUsername, 
                    senderUsername: senderUsername 
                })
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error("Failed to confirm friend request.");
                    }

                    alert('The friend request confirmed successfully!');
                })
                .catch(error => {
                    console.error('Error:', error);
                    alert('An error occurred while confirming the friend request.');
                });
        });
    } else {
        console.error('Button with id "confirm" not found!');
    }
});