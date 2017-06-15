<script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyAT8DIEC_kxOQbKy5hhFKdydtcuSkxCWIs"></script>
    <script type="text/javascript">
        var markers = @Html.Raw(ViewBag.Markers);
        window.onload = function () {
            var mapOptions = {
            center: new google.maps.LatLng(markers[0].lat, markers[0].lng),
                zoom: 8,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            var infoWindow = new google.maps.InfoWindow();
            var map = new google.maps.Map(document.getElementById("map"), mapOptions);
            for (i = 0; i < markers.length; i++) {
                var data = markers[i]
                var myLatlng = new google.maps.LatLng(data.lat, data.lng);
                var marker = new google.maps.Marker({
            position: myLatlng,
                    map: map,
                    title: data.title
                });
                (function (marker, data) {
            google.maps.event.addListener(marker, "click", function (e) {
                infoWindow.setContent(data.description);
                infoWindow.open(map, marker);
            });
        })(marker, data);
            }
        }
    </script>