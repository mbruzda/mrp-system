//alert("aaa");

const data = JSON.stringify({
    name: 'Roger',
    age: 8
  })
  var result
  var text = "{'salesForecast':[0,0,0,0,20,0,40,0,0,0],'production':[0,0,0,0,28,0,30,0,0,0],'inventory':[0,0,0,0,0,0,0,0,0,0],'realizationTime':1,'startingInventory':2}";

  const xhr = new XMLHttpRequest()
  xhr.withCredentials = false;
  
  xhr.addEventListener('readystatechange', function() {
    if (this.readyState === this.DONE) {
      result = JSON.parse(this.response)
      ShowResult()
    }
  })
  
  xhr.open('POST', 'https://localhost:44362/api/GetGHPTable', true)
  xhr.setRequestHeader('content-type', 'application/json')
  xhr.withCredentials = false;
  
  xhr.send(JSON.stringify(text))


  function ShowResult(){
    console.log(result.SalesForecast)
  }
  