<template>
    <div>
        <video id="video" class="video-js" controls preload="auto" width="1280" height="720">
            <source src="http://192.168.21.100:8080/live/livestream.m3u8" type="application/x-mpegURL">
        </video>
    </div>
</template>



<script>
    import { onMounted, computed } from "vue";
    import videojs from 'video.js';
    import './../../node_modules/video.js/dist/video-js.css';

    export default {
        name: "RTSP",
        setup() {

            onMounted(() => {
                videojs(document.getElementById('video'));

            });

            return {

            };
        }
    }
</script>

<style scoped>
</style>