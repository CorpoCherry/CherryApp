/// <binding />
"use strict";
const ExtractTextPlugin = require('extract-text-webpack-plugin');
const MinifyPlugin = require("babel-minify-webpack-plugin");
const UglifyJsPlugin = require('uglifyjs-webpack-plugin');
const webpack = require('webpack');
const fs = require('fs');
const {  join, dirname, resolve } = require('path');
const extractSCSS = new ExtractTextPlugin('[name]');
const rimraf = require('rimraf');
const CopyWebpackPlugin = require('copy-webpack-plugin');
const Production = (process.env.NODE_ENV === 'production');

ClearRoot();

var AdditionalFiles = 
    [
        {
            name: '/sw.js',
            path: './Scripts/sw.js'
        }
    ]

function ClearRoot() {
    var x = fs.readdirSync('wwwroot');
    x.forEach(element => {
        rimraf('wwwroot/' + element, function () { console.log('Cleared wwwroot/' + element); });
    });
}
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
function getGlobalScripts() {
    return fs.readdirSync('./Scripts/Global/')
        .filter(
        (file) => file.match(/.*\.js$/)
        )
        .map((file) => {
            return {
                name: 'js/global/' + file.substring(0, file.length - 3).toLowerCase() + ReturnGoodFileEnd("js"),
                path: './Scripts/Global/' + file
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
function getAll() {
    var scripts = getScripts();
    var styles = getStyles();
    var stylesg = getGlobalScripts();
    var all = scripts.concat(styles).concat(stylesg).concat(AdditionalFiles);

    return all.reduce((memo, file) => {
        memo[file.name] = file.path;
        return memo;
    }, {});
}
function getFontsLoader() {
    return Production ? 'file-loader?name=/fonts/[name].[hash].[ext]' : 'file-loader?name=/fonts/[name].[ext]';
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
                    test: /\.(eot|svg|ttf|woff|woff2)$/,
                    loader: getFontsLoader
                },
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
            new webpack.ProvidePlugin({
            $: "jquery"
            }),
            extractSCSS,
            new CopyWebpackPlugin(
                [
                    {
                        from: './Other/Manifests/manifest.json'
                    },
                    {
                        from: './Other/Manifests/browserconfig.xml'
                    },
                    {
                        from: './Other/Images/Touch',
                        to: 'img/touch'
                    }
                ])
        ],
        optimization: {
            minimize: false
        }
    };

if (Production) {
    console.log("Production MODE");
    config.plugins.push(
            new webpack.optimize.ModuleConcatenationPlugin()
    );
    config.optimization.minimize = true;
}
else {
    console.log("Development MODE");
    config.output.library = 'mdc';
    config.output.libraryTarget = 'umd';
}

module.exports = config;