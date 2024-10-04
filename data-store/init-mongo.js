db = db.getSiblingDB('exampledb');

// Drop the database to clear data
db.dropDatabase();

db.createUser({
    user: "mongouser",
    pwd: "mongopass",
    roles: [{ role: "readWrite", db: "exampledb" }]
});