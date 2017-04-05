$('#changeImage').on('click', function (e) { $('#txtUploadFile').click(); });
$('#priceEditor').on('change', function (e) {
	var price = $('#priceEditor').val();
	$('#price').html(price + " руб");
});

$('#txtUploadFile').on('change', function (e) {
	var files = e.target.files;
	var fileName = $('#txtUploadFile').attr("fileName");
	if (files.length > 0) {
		if (window.FormData !== undefined) {
			var data = new FormData();
			for (var x = 0; x < files.length; x++) {
				data.append("file" + x, files[x]);
			}

			$.ajax({
				type: "POST",
				url: '/CatalogEditor/UploadFile?id=' + fileName,
				contentType: false,
				processData: false,
				data: data,
				success: function (result) {
					console.log(result);
					var extention = getFileExtension(files[0].name);
					$('#Image').val("/Content/images/catalog/IMG" + fileName + "." + extention);
					$('#ImagePreview').val("/Content/images/catalog/IMG" + fileName + "." + extention + ".[small]." + extention);
					$('#imageLink').attr("href", "/Content/images/catalog/IMG" + fileName + "." + extention + "?timestamp=" + new Date().getTime());
					$('#imagePreView').attr("src", "/Content/images/catalog/IMG" + fileName + "." + extention + ".[small]." + extention + "?timestamp=" + new Date().getTime());
				},
				error: function (xhr, status, p3, p4) {
					var err = "Error " + " " + status + " " + p3 + " " + p4;
					if (xhr.responseText && xhr.responseText[0] == "{")
						err = JSON.parse(xhr.responseText).Message;
					console.log(err);
				}
			});
		} else {
			alert("This browser doesn't support HTML5 file uploads!");
		}
	}
});


function getFileExtension(filename) {
	return filename.slice((filename.lastIndexOf(".") - 1 >>> 0) + 2);
}