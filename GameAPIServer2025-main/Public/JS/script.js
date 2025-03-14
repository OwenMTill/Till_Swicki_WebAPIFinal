const playerContainer = document.getElementById("player-list")

const fetchPlayers = async()=>{
    var index =0;
    try{
        const responce = await fetch("/player");

        if(!responce.ok){
            throw new Error("Failed to get players");
        }

        const player = await responce.json();

        if(window.location.pathname ==="/leaderboard")
        {
            player.sort((a, b)=>b.highscore - a.highscore);
        }

        // console.log(items);
        playerContainer.innerHTML = "";
        
        player.forEach((player) => {
            const playerDiv = document.createElement("div");
            playerDiv.className = "player";
            
            // if(window.location.pathname ==="/")
            // {
            //     playerDiv.innerHTML =`<li>Username: ${player.username} <br>
            //     Games Played: ${player.gamesplayed}
            //     <br><br>
            //     </li>`
            //     playerContainer.appendChild(playerDiv);
            // }

            switch(window.location.pathname)
            {
                case "/":
                    playerDiv.innerHTML =`<li>Username: ${player.username} <br>
                    Games Played: ${player.gamesplayed}
                    <br><br>
                    </li>`
                    playerContainer.appendChild(playerDiv);
                    break;
                case "/update":
                    playerDiv.innerHTML =`<li>Username: ${player.username} <br>
                    Games Played: ${player.gamesplayed}<br>
                    <button onclick="updatePlayer('${player.playerid}')">Update</button> 
                    <button onclick="deletePlayer('${player.playerid}')">Delete</button>
                    </li>`
                    playerContainer.appendChild(playerDiv);
                    break;
                case "/leaderboard":
                    
                    if(index <=9)
                    {
                        playerDiv.innerHTML =`<li>Username: ${player.username} <br>
                        Games Played: ${player.gamesplayed}<br>
                        Last Score: ${player.score}<br>
                        High Score: ${player.highscore}<br>
                        Win/Loss: ${player.win}/${player.loss}<br><br>
                        </li>`
                        playerContainer.appendChild(playerDiv);
                        index++;
                    }
                    
                    break;

            }
            // else{
            //     itemDiv.innerHTML =`<li>${item.item} </li>`
            // itemContainer.appendChild(itemDiv);
            // }
            
        });

    }catch(err){
        console.error("Error: ",err);
        playerContainer.innerHTML="<p style='color:red'>Failed to get players</p>";
    }
}

const updatePlayer = async(id)=>{
    try{
        console.log(id);
        window.location.href = `/update/${id}`;
        // const responce = await fetch(`/update/${id}`);
    }catch(err){
        console.error("Failed to connect");
    }
}

const deletePlayer = async(id)=>{
    if(!confirm("Are you sure you want to delete this?"))return;
    try{
        console.log(id);
        const responce = await fetch(`/deletedata/${id}`,{method: 'DELETE'});
        
        if(!responce.ok){
            throw new Error("Failed to delete");
        }

        fetchPlayers();
    }catch(err){
        console.error("Error deleteing item: ",err)
    }
}

fetchPlayers();