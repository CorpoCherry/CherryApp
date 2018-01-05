/// <binding BeforeBuild='Run - Development' />
"use strict";
const ExtractTextPlugin = require('extract-text-webpack-plugin');
const MinifyPlugin = require("babel-minify-webpack-plugin");
const UglifyJsPlugin = require('uglifyjs-webpack-plugin');
const webpack = require('webpack');
const fs = require('fs');
const {  join, dirname, resolve } = require('path');
const extractSCSS = new ExtractTextPlugin('[name]');

const Production = (process.env.NODE_ENV === 'production');

function ReturnGoodFileEnd(name) {
    if (Production) {
        return ".min." + name;
    }
    else {
        return "." + name;
    }
}

function getScripts() {
    return fs.readdirSync('./Scripts/Pages/')
        .filter(
        (file) => file.match(/.*\.js$/)
        )
        .map((file) => {
            return {
                name: 'js/pages/' + file.substring(0, file.length - 3).toLowerCase() + ReturnGoodFileEnd("js"),
                path: './Scripts/Pages/' + file
            };
        });
}
function getStyles() {
    return fs.readdirSync('./Styles/Pages/')
        .filter(
        (file) => file.match(/.*\.scss$/)
        )
        .map((file) => {
            return {
                name: 'css/pages/' + file.substring(0, file.length - 5).toLowerCase() + ReturnGoodFileEnd("css"),
                path: './Styles/Pages/' + file
            };
        });
}

/*
.reduce((memo, file) => {
    memo[file.name] = file.path;
    return memo;
}, {})
*/

function getAll() {
    var scripts = getScripts();
    var styles = getStyles();
    var all = scripts.concat(styles);

    return all.reduce((memo, file) => {
        memo[file.name] = file.path;
        return memo;
    }, {});
}

var config =
    {
        entry: getAll,
        output: {
            path: resolve(__dirname, 'wwwroot'),
            filename: '[name]'
        },
        module:
        {
            rules:
            [
                {
                    test: /\.scss$/,
                    use: ExtractTextPlugin.extract(
                    {
                        fallback: 'style-loader',
                        use:
                        [
                            {
                                loader: 'css-loader',
                                options:
                                {
                                    minimize: Production
                                }
                            },
                            {
                                loader: 'sass-loader',
                                options:
                                {
                                    includePaths:
                                    [
                                        join(dirname(module.filename), 'node_modules')
                                    ]
                                }
                            }
                        ]
                    })   
                },
                {
                    test: /\.js$/,
                    use: 
                    [
                        {
                            loader: 'babel-loader',
                            query: {
                                presets:
                                [
                                    'es2015',
                                    'env'
                                ]
                            }
                        }
                    ]
                }
            ]
        },
        target: "web",
        plugins:
        [
            extractSCSS
        ]
    };

if (Production) {
    console.log("Production MODE");
    config.plugins.push(
            new webpack.optimize.ModuleConcatenationPlugin(),
            new UglifyJsPlugin()
    );
    
    //config.module.rules[1].use[0].query.presets.push('minify');
}
else {
    console.log("Development MODE");
    config.output.library = 'mdc';
    config.output.libraryTarget = 'umd';
}

module.exports = config;