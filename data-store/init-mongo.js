db = db.getSiblingDB('exampledb');

db.createUser({
    user: "mongouser",
    pwd: "mongopass",
    roles: [{ role: "readWrite", db: "exampledb" }]
});
