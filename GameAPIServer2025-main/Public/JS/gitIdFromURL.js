const path = window.location.pathname;

const pathSegments = path.split('/');

const playerId = pathSegments[pathSegments.length-1];
console.log(playerId);