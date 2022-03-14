//alert("aaa");

const data = JSON.stringify({
    name: 'Roger',
    age: 8
  })
  
  var text = "{'salesForecast':[0,0,0,0,20,0,40,0,0,0],'production':[0,0,0,0,28,0,30,0,0,0],'inventory':[0,0,0,0,0,0,0,0,0,0],'realizationTime':1,'startingInventory':2}";

  const xhr = new XMLHttpRequest()
  xhr.withCredentials = true
  
  xhr.addEventListener('readystatechange', function() {
    if (this.readyState === this.DONE) {
      console.log(this.responseText)
    }
  })
  
  xhr.open('POST', 'https://localhost:44362/api')
  xhr.setRequestHeader('content-type', 'application/json')
  xhr.setRequestHeader('authorization', 'Bearer 123abc456def')
  
  xhr.send(data)