const path = require("path");


module.exports = {
    entry: {
        "Index": "./src/ts/Index.ts",
        "Main": "./src/ts/Main.ts",
    },
    output: {
        path: path.resolve(__dirname, "dist"),
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