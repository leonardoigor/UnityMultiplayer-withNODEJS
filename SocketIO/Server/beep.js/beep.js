var io = require("socket.io")({
  transports: ["websocket"]
});

io.attach(4567);
var players = [];
io.on("connection", function(socket) {
  console.log("alguem entrou");
  var currentPlayer = [];
  socket.on("Start", data => {
    const { name, life, p_x, p_y, p_z, l_x, l_y, l_z, l_w } = data;
    const newUser = {
      name,
      life,
      p_x,
      p_y,
      p_z,
      l_x,
      l_y,
      l_z,
      l_w,
      id: socket.id
    };
    players.push(newUser);
    currentPlayer = newUser;
    // console.log(newUser);
    socket.broadcast.emit("Start", currentPlayer);
    socket.emit("Start", currentPlayer);
    for (let i in players) {
      if (players[i].id != currentPlayer.id) {
        socket.emit("Start", players[i]);
        console.log("START");
      }
    }
  });
  socket.on("Move", data => {
    for (let i in players) {
      if (players[i].id == data.name) {
        players[i].p_x = data.p_x;
        players[i].p_y = data.p_y;
        players[i].p_z = data.p_z;
        players[i].l_x = data.l_x;
        players[i].l_y = data.l_y;
        players[i].l_z = data.l_z;
        players[i].l_w = data.l_w;
        socket.broadcast.emit("Move", players[i]);
        // console.log(players[i]);
      }
    }
  });
  socket.on("LifeChange", data => {
    for (let i in players) {
      if (data.id == players[i].id) {
        players[i].life = data.life;
        socket.broadcast.emit("LifeChange", data);
        socket.emit("LifeChange", data);
      }
    }
  });

  socket.on("disconnect", () => {
    for (let i in players) {
      if (players[i].id == currentPlayer.id) {
        socket.broadcast.emit("destroi", currentPlayer);
        players.splice(i, 1);
      }
    }
  });
});
console.log("--running--");
