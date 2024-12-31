document.addEventListener('DOMContentLoaded', function () {
    const confirmButton = document.getElementById('confirm');
    const deleteButton = document.getElementById('delete');
    
    const currentUserUsername = window.currentUsername;

    if (confirmButton) {
        confirmButton.addEventListener('click', function () {
            const senderUsername = confirmButton.getAttribute('data-username');
            const parentNotification = this.closest('.notification-wrapper');

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

                    if (parentNotification) {
                        parentNotification.style.display = 'none';
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

    if (deleteButton) {
        deleteButton.addEventListener('click', function () {
            const senderUsername = deleteButton.getAttribute('data-username');
            const parentNotification = this.closest('.notification-wrapper');

            fetch(`https://localhost:7018/api/FriendRequestApi/DeleteRequest`, {
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
                        throw new Error("Failed to delete the friend request.");
                    }

                    if (parentNotification) {
                        parentNotification.style.display = 'none';
                    }

                    alert('The friend request deleted successfully!');
                })
                .catch(error => {
                    console.error('Error:', error);
                    alert('An error occurred while deleting the friend request.');
                });
        });
    } else {
        console.error('Button with id "delete" not found!');
    }

});