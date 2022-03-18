//alert("aaa");

var result

var text = "{'salesForecast':[0,0,0,0,10,0,20,0,0,0],'production':[0,0,0,0,0,0,0,0,0,0],'inventory':[0,0,0,0,0,0,0,0,0,0],'realizationTime':1,'startingInventory':2}";



const xhr = new XMLHttpRequest()

xhr.withCredentials = false;

function sendData(){

    text = "{'salesForecast':["+
    document.getElementById("sale1").value+","+
    document.getElementById("sale2").value+","+
    document.getElementById("sale3").value+","+
    document.getElementById("sale4").value+","+
    document.getElementById("sale5").value+","+
    document.getElementById("sale6").value+","+
    document.getElementById("sale7").value+","+
    document.getElementById("sale8").value+","+
    document.getElementById("sale9").value+","+
    document.getElementById("sale10").value+
    ",],'production':["+
    document.getElementById("production1").value+","+
    document.getElementById("production2").value+","+
    document.getElementById("production3").value+","+
    document.getElementById("production4").value+","+
    document.getElementById("production5").value+","+
    document.getElementById("production6").value+","+
    document.getElementById("production7").value+","+
    document.getElementById("production8").value+","+
    document.getElementById("production9").value+","+
    document.getElementById("production10").value+","+
    "],'inventory':[0,0,0,0,0,0,0,0,0,0],'realizationTime':"+document.getElementById("time").value+
    ",'startingInventory':"+document.getElementById("inventory").value+"}";
    xhr.addEventListener('readystatechange', function () {
    if (this.readyState === this.DONE) {
      try{
        result = JSON.parse(this.response)
      }catch{
        console.log(this.response)
        //Here will be code that will make popup with response message
      }
      
      ShowResult()
      
    }
  
  })
  
  xhr.open('POST', 'https://35.246.143.214/api/GetGHPTable', true)
  xhr.setRequestHeader('content-type', 'application/json')
  xhr.withCredentials = false;

  xhr.send(JSON.stringify(text))

}

  



function ShowResult() {

  console.log(result)

  for(var i =0; i<10; i++){
    if(result.SalesForecast[i]!=0){
       document.getElementById("saleTable"+(i+1)).innerHTML = result.SalesForecast[i]
    }else 
    {
      document.getElementById("saleTable"+(i+1)).innerHTML = "";
    }

    if(result.Production[i]!=0){
      document.getElementById("productionTable"+(i+1)).innerHTML = result.Production[i]
   }else 
   {
     document.getElementById("productionTable"+(i+1)).innerHTML = "";
   }
   
    document.getElementById("inventoryTable"+(i+1)).innerHTML = result.Inventory[i]
  }
}
