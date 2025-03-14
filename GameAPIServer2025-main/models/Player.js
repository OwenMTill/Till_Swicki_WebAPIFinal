const mongoose = require("mongoose");

const playerSchema = new mongoose.Schema({
    playerid:{ type: String, unique:true},
    username:String,
    score:Number,
    highscore:Number,
    gamesplayed:Number,
    win:Number,
    loss:Number
})

const Player = mongoose.model("Player", playerSchema);

module.exports = Player