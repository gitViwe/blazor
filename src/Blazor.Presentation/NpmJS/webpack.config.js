const path = require('path');

module.exports = {
    entry : "./src/open_telemetry.ts",
    output : {
        path: path.resolve(__dirname, "../wwwroot/js"),
        filename: "open_telemetry.bundle.js"
    },
    module: {
        rules: [
          {
            test: /\.tsx?$/,
            use: 'ts-loader',
            exclude: /node_modules/,
          },
        ],
    },
    resolve: {
        extensions: ['.tsx', '.ts', '.js'],
    }
};