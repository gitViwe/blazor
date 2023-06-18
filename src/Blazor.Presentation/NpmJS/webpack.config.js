const path = require('path');

module.exports = (env) => {
    return {
      entry: {
        index: env.IsLocal ? './src/index.local.ts' : './src/index.ts',
        open_telemetry: './src/open_telemetry.ts',
      },
      output : {
        path: path.resolve(__dirname, "../wwwroot/js"),
        filename: "[name].bundle.js"
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
  }
};