﻿//import { Point } from "mapbox-gl";

$(function () {
    
    msg_el = document.getElementById('msg'),
    vectorSource = new ol.source.Vector(),
    vectorLayer = new ol.layer.Vector({
        source: vectorSource
    }),

    url_osrm_nearest = '//router.project-osrm.org/nearest/v1/driving/',
    url_osrm_route = '//router.project-osrm.org/route/v1/driving/',
    icon_url = 'https://raw.githubusercontent.com/openlayers/ol3/master/examples/data/icon.png',
    styles = new ol.style.Style({
        image: new ol.style.Icon({
            anchor: [0.5, 1],
            src: icon_url,
            anchor: [0.5, 46],
            anchorXUnits: 'fraction',
            anchorYUnits: 'pixels',
            opacity: 0.75,
        })
    }),
    points = [],
    // 56.9246204 24.105487699999998

    createFeature = function (coord) {
        var feature = new ol.Feature({
            type: 'point',
            //geometry: new ol.geom.Point(ol.proj.fromLonLat(coord))
            geometry: new ol.geom.Point(ol.proj.transform(coord, 'EPSG: 4326', 'EPSG: 3857'))//{ 0:coord[i].Lon, 1: }ol.proj.fromLonLat(coord)//[coord[0], coord[1]]
        });
        feature.setStyle(styles);
        vectorSource.addFeature(feature);
    },

    to4326 = function (coord) {
        return ol.proj.transform([
            parseFloat(coord[0]), parseFloat(coord[1])
        ], 'EPSG:3857', 'EPSG:4326');
    },

    view = new ol.View({
        center: [0, 0],
        zoom: 2
    }),

    map = new ol.Map({
        layers: [
            new ol.layer.Tile({
                source: new ol.source.OSM()
            }),
            vectorLayer
        ],
        target: 'map',
        controls: ol.control.defaults({
            //attributionOptions: {
            //    collapsible: false
            //}
        }),
        view: view
    });

    map.on('click', function (evt) {
   
    });

    //TEST DRAW
    //var thing = new ol.geom.Polygon([[
    //    ol.proj.transform([-16, -22], 'EPSG:4326', 'EPSG:3857'),
    //    ol.proj.transform([-44, -55], 'EPSG:4326', 'EPSG:3857')
    //]]);
    //var featurething = new ol.Feature({
    //    name: "Thing",
    //    geometry: thing
    //});
    //vectorSource.addFeature(featurething);
    /*
    stylePointFunction = function (custom_img) {
        //var src = "/_dgv/proxy.img.php?paper_blueprint:" + custom_img;
        var icon = new ol.style.Icon({
            src: icon_url,
            anchor: [0.5, 32],
            anchorXUnits: "fraction",
            anchorYUnits: "pixels",
            size: [36, 36],
            opacity: 1,
        })
    };
    drawStyle = stylePointFunction('4i.png');
    //draw = new ol.interaction.Draw({
    //    source: vectorSource,
    //    style: drawStyle,
    //    type: "Point",
    //    //geometryFunction: geometryFunction,
    //});
    function addInteraction() {
            draw = new ol.interaction.Draw({
                source: vectorSource,
                type: "point",
                style:drawStyle
            });
            map.addInteraction(draw);
    }
    addInteraction();
    */
    //var geolocation = new ol.Geolocation({
    //    projection: view.getProjection()
    //});

    //function el(id) {
    //    return document.getElementById(id);
    //}

    //el('track').addEventListener('change', function () {
    //    geolocation.setTracking(this.checked);
    //});

    //// update the HTML page when the position changes.
    //geolocation.on('change', function () {
    //    el('accuracy').innerText = geolocation.getAccuracy() + ' [m]';
    //    el('altitude').innerText = geolocation.getAltitude() + ' [m]';
    //    el('altitudeAccuracy').innerText = geolocation.getAltitudeAccuracy() + ' [m]';
    //    el('heading').innerText = geolocation.getHeading() + ' [rad]';
    //    el('speed').innerText = geolocation.getSpeed() + ' [m/s]';
    //});

    //// handle geolocation error.
    //geolocation.on('error', function (error) {
    //    var info = document.getElementById('info');
    //    info.innerHTML = error.message;
    //    info.style.display = '';
    //});

    //var accuracyFeature = new ol.Feature();
    //geolocation.on('change:accuracyGeometry', function () {
    //    accuracyFeature.setGeometry(geolocation.getAccuracyGeometry());
    //});

    //var positionFeature = new ol.Feature();
    //positionFeature.setStyle(new ol.style.Style({
    //    image: new ol.style.Circle({
    //        radius: 6,
    //        fill: new ol.style.Fill({
    //            color: '#3399CC'
    //        }),
    //        stroke: new ol.style.Stroke({
    //            color: '#fff',
    //            width: 2
    //        })
    //    })
    //}));

    //geolocation.on('change:position', function () {
    //    var coordinates = geolocation.getPosition();
    //    positionFeature.setGeometry(coordinates ?
    //        new ol.geom.Point(coordinates) : null);
    //});

    //new ol.layer.Vector({
    //    map: map,
    //    source: new ol.source.Vector({
    //        features: [accuracyFeature, positionFeature]
    //    })
    //});

    function sendToController_v1(method) {
        $.ajax({
            url: "Home/" + method,
            type: "POST",
            datatype: 'json',
            //data: { userData: JSON.stringify(pageData) },
            success: function (e) {
                console.log(e);

                drawUsers_v1(JSON.parse(e));

            },
            error: function (err) {
                console.log(err);
                alert(err);
            }
        })
    }
    function drawUsers_v1(coord) {
        //TD:Change
        var iconStyle = new ol.style.Style({

            image: new ol.style.Circle({
                radius: 3,
                fill: new ol.style.Fill({
                    color: [255, 204, 102, 1]
                }),
                stroke: new ol.style.Stroke({
                    color: [255, 204, 102, 1],
                    width: 1.5
                })
            }),
            zIndex: 1

        });

        //vectorSource.clear();

        var thing = new ol.geom.Polygon([[
            ol.proj.transform([-16, -22], 'EPSG:4326', 'EPSG:3857'),
            ol.proj.transform([-44, -55], 'EPSG:4326', 'EPSG:3857')
        ]]);
        var featurething = new ol.Feature({
            name: "Thing",
            geometry: thing
        });
        vectorSource.addFeature(featurething);

        for (var i in coord) {

            //createFeature([coord[i].Lon,coord[i].Lat]);

            //var addedMarker = new ol.geom.Point([
            //    ol.proj.transform([coord[i].Lon, coord[i].Lat], 'EPSG:4326', 'EPSG:3857')]);


            //var featurething = new ol.Feature({
            //    name: "Marker 01",
            //    geometry: addedMarker
            //});

            //featurething.setStyle(iconStyle);

            //vectorSource.addFeature(featurething);

        }


        //usersFeature.setStyle(iconStyle);
        //vectorSource.addFeature(usersFeature);
        //vectorSource.addFeatures(usersFeature);

    }
    console.log(113);

    sendToController_v1("GetUsers");
})