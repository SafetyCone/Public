const path = require("path");


module.exports = {
    entry: {
        //"Main": "./src/ts/Main.ts",
        "Test": "./src/ts/Test.ts"
    },
    output: {
        path: path.resolve(__dirname, "wwwroot/dist"),
        filename: "[name].js",
        publicPath: "/"
    },
    resolve: {
        extensions: [".js", ".ts"]
    },
    module: {
        rules: [
            {
                test: /\.ts$/,
                use: "ts-loader"
            },
        ]
    }
};