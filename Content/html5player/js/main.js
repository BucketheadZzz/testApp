/**
 *
 * HTML5 Audio player with playlist
 *
 * Licensed under the MIT license.
 * http://www.opensource.org/licenses/mit-license.php
 * 
 * Copyright 2012, Script Tutorials
 * http://www.script-tutorials.com/
 */
jQuery(document).ready(function() {

    // inner variables

    $.fn.myPlayer = function() {
        var song;
        var self = $(this);
        var tracker = $(this).find('.tracker');
        var volume = $(this).find('.volume');
        var playControl = $(this).find('.play');
        var stopControl = $(this).find('.pause');
        var fwdControl = $(this).find('.fwd');
        var playListShow = $(this).find('.pl');
        var rewControl = $(this).find('.rew');
        var songSelect = $('.playlist[data-parent-player="' + self.attr("id") + '"] li');
        var playListItem = $('.playlist[data-parent-player="' + self.attr("id") + '"] li');
        var playList = $('.playlist[data-parent-player="' + self.attr("id") + '"]');

       

        function initAudio(elem) {
            var url = elem.attr('audiourl');
            var title = elem.text();
            var artist = elem.attr('artist');

            


            self.find('.title').text(title);
            self.find('.artist').text(artist);

            song = new Audio(url);

            // timeupdate event listener
            song.addEventListener('timeupdate', function () {
                var curtime = parseInt(song.currentTime, 10);
                tracker.slider('value', curtime);
            });

            playListItem.removeClass('active');
            elem.addClass('active');
        }
        function playAudio() {
            song.play();

            tracker.slider("option", "max", song.duration);

            playControl.addClass('hidden');
            stopControl.addClass('visible');
        }
        function stopAudio() {
            song.pause();

            playControl.removeClass('hidden');
            stopControl.removeClass('visible');
        }

        // play click
        playControl.click(function (e) {
            e.preventDefault();

            playAudio();
        });

        // pause click
        stopControl.click(function (e) {
            e.preventDefault();

            stopAudio();
        });

        // forward click
        fwdControl.click(function (e) {
            e.preventDefault();

            stopAudio();

            var next = playList.find('li.active').next();
            if (next.length == 0) {
                next = playList.find('li:first-child');
            }
            initAudio(next);
        });

        // rewind click
        rewControl.click(function (e) {
            e.preventDefault();

            stopAudio();

            var prev = playList.find('li.active').prev();
            if (prev.length == 0) {
                prev = playList.find('li:last-child');
            }
            initAudio(prev);
        });

        // show playlist
        playListShow.click(function (e) {
            e.preventDefault();
            playList.fadeToggle(300);
        });

        // playlist elements - click
        songSelect.click(function () {
            stopAudio();
            initAudio($(this));
        });

        // initialization - first element in playlist
        initAudio($('.playlist[data-parent-player="' + self.attr("id") + '"] li:first-child'));

        // set volume
        song.volume = 0.8;

        // initialize the volume slider
        volume.slider({
            range: 'min',
            min: 1,
            max: 100,
            value: 80,
            start: function (event, ui) { },
            slide: function (event, ui) {
                song.volume = ui.value / 100;
            },
            stop: function (event, ui) { },
        });

        // empty tracker slider
        tracker.slider({
            range: 'min',
            min: 0, max: 10,
            start: function (event, ui) { },
            slide: function (event, ui) {
                song.currentTime = ui.value;
            },
            stop: function (event, ui) { }
        });
    }

  
});
