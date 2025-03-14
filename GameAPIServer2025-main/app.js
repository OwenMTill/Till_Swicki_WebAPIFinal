require("dotenv").config();
const express = require("express");
const mongoose = require("mongoose");
const bodyParser = require("body-parser");
const path = require("path");
const session = require("express-session");
const fs = require("fs");
const cors = require("cors");
const { nanoid } = require("nanoid");
const Player = require("./models/Player");

const app = express();
const port = process.env.port || 3000;

app.use(express.json());
app.use(cors()); //Allows us to make requiests from our game.
app.use(bodyParser.json());

app.use(express.urlencoded({extended:true}));

app.use(session({
    secret: process.env.SESSION_SECRET,
    resave:false,
    saveUninitialized:false,
    cookie:{secure:false}// Set to true is using https
}));

app.use(express.static(path.join(__dirname, 'public')));

const FILE_PATH = "player.json";

//Connection for MongoDB
mongoose.connect(process.env.MONGODB_URI.toString());//"mongodb://localhost:27017/gamedb");

const db = mongoose.connection;

db.on("error", console.error.bind(console, "MongoDB connection error"));
db.once("open", ()=>{
    console.log("Connected to MongoDB Database");
});


//API endpoint for player.json;

// app.get("/player", (req,res)=>{
//     fs.readFile(FILE_PATH, "utf-8",(err, data)=>{
//         if(err){
//             return res.status(500).json({error:"Unable to fetch data"});
//         }
//         res.json(JSON.parse(data));
//         console.log(`Responded with: ${data}`);
//     })
// });
app.get("/", (req,res)=>{
    res.sendFile(path.join(__dirname,"public","index.html"));
});

app.get("/addplayer", (req,res)=>{
    res.sendFile(path.join(__dirname,"public","addplayer.html"));
});

app.get("/update", (req,res)=>{
    res.sendFile(path.join(__dirname,"public","update.html"));
});

app.get("/leaderboard", (req,res)=>{
    res.sendFile(path.join(__dirname,"public","leaderboard.html"));
});

app.get("/player", async (req,res)=>{
    try{
        const players = await Player.find();
        if(!players){
            return res.status(404).json({error:"Players not found"});
        }

        res.json(players);
        //console.log(players);
    }
    catch(error){
        res.status(500).json({error:"Failed to retrieve players"})
    }
});

app.get("/player/:param", async (req,res)=>{
    console.log(req.params.param)
    const param = req.params.param;

    let player = null;
    
    try{
        player = await Player.findOne({ playerid: param });
        
        // If no player was found by playerid, check if the param matches a username
        if (!player) {
            player = await Player.findOne({ username: param });
        }

        if(!player){
          
                return res.status(404).json({error:"Player not found"});
            
        }
        res.json(player);
    }
    catch(error)
    {
        res.status(500).json({error:"Failed to retrieve player"})
    }
});

// app.get("/player/:username", async(req,res)=>{
//     console.log(req.params.username);
//     try{
        
//         const player = await Player.findOne({username:req.params.username});

//         if(!player){
//             return res.status(404).json({error:"Player not found"})
//         }
//         res.json(player);

//     }
//     catch(error)
//     {
//         res.status(500).json({error:"Failed to retrieve player"})
//     }
// });

// app.get("/player/:playerid", async(req,res)=>{
//     console.log(req.params.playerid);
//     try{

//         const player = await Player.findOne({playerid:req.params.playerid});

//         if(!player){
//             return res.status(404).json({error:"Player not found"})
//         }
//         res.json(player);

//     }
//     catch(error)
//     {
//         res.status(500).json({error:"Failed to retrieve player"})
//     }
// });

app.post("/sentdata", (req,res)=>{
    const newPlayerData = req.body;

    //console.log(JSON.stringify(newPlayerData,null,2));

    res.json({message:"Player Data recieved"});
});

app.post("/sentdatatodb", async (req,res)=>{
    try{
        const newPlayerData = req.body;

        //console.log(JSON.stringify(newPlayerData,null,2));

        const existingPlayer = await Player.findOne({username: newPlayerData.username});

        if(existingPlayer)
        {
            return res.send("<p>Username already taken. Try a different one</p><br><a href='/addplayer.html'>Back</a>");
        }

        const newPlayer = new Player({
            playerid:nanoid(8),
            username:newPlayerData.username,
            score:newPlayerData.score,
            highscore:newPlayerData.highscore,
            gamesplayed:newPlayerData.gamesplayed,
            win:newPlayerData.win,
            loss:newPlayerData.loss

        });
        if(newPlayer.highscore<newPlayer.score)
        {
            newPlayer.highscore = newPlayer.score;
        }
        //save to database
        await newPlayer.save();
        res.redirect("/");
        //res.json({message:"Player Added Successfully",playerid:newPlayer.playerid, name:newPlayer.name});
    }
    catch(error){
        res.status(500).json({error:"Failed to add player"})
    }
});

//Update Player
app.post("/updatePlayer/:username", async(req,res)=>{
    const username = req.params.username; 
    const playerData = req.body;
    const player = await Player.findOne({username:username});

    if(!player){
        return res.status(404).json({message:"Player not found"});
    }
    
    player.score = playerData.score;

    if(player.score>player.highscore)
    {
        player.highscore = player.score;
    }

    player.gamesplayed = playerData.gamesplayed;
    player.win = playerData.win;
    player.loss = playerData.loss;

    await player.save();

    res.json({message:"Player updated", player});
});

app.get("/update/:playerid", async (req,res)=>{
    try{
        const playerIdFromUrl = req.params.playerid;

        const filePath = path.join(__dirname, "public", "updateplayer.html");
        console.log(filePath);
        res.sendFile(filePath,{
            headers:{
                'Cache-Control':'no-store'
            }
        });

    }catch(err){
        res.status(500).json({error:"server error"});
    }
});

app.post("/updatePlayer/:playerid", async (req, res)=>{
    const playerId = req.params.playerid;
    const updatedPlayer = req.body; 
    console.log(updatedPlayer);

    try{
        const newName = await Player.findOneAndUpdate({playerid: playerId},{username:updatedPlayer.username},{new: true});
        if (!newName) {
            return res.status(404).send("Item not found"); 
        }
        res.redirect('/');
    }catch(error){
        console.error("Error updating item:", error);
        res.status(500).send("Server error while updating item");
    }
    
});

app.delete("/deletedata/:playerid", async (req,res)=>{
    try{
        const playerID = await Player.findOne({playerid:req.params.playerid});
        
        console.log(playerID);

        if (!playerID) {
            return res.status(400).json({ error: "Could not find player." });
        }

        console.log("Deleting player with ID:", req.params.playerid);

        // Deleting the player
        const deletedPlayer = await Player.findOneAndDelete( {playerid: req.params.playerid} );
        
        if (!deletedPlayer) {
            console.log("Player not deleted:", deletedPlayer);
            return res.status(400).json({ error: "Failed to delete the player" });
        }

        console.log("Deleted player: ",deletedPlayer);
        
        res.json({ message: "Player deleted successfully" });
    } catch (err) {
        console.log(err);
        res.status(500).json({ error: "An error occurred while deleting the player." });
    }
});

app.listen(3000, ()=>{
    console.log("Running on port 3000");
})