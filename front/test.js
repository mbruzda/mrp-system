//alert("aaa");

var result
var wMRP
var text = "{'salesForecast':[0,0,0,0,10,0,20,0,0,0],'production':[0,0,0,0,0,0,0,0,0,0],'inventory':[0,0,0,0,0,0,0,0,0,0],'realizationTime':1,'startingInventory':2}";
var mrp = "{'grossRequirements':[0,0,0,0,10,0,20,0,0,0],'scheduledReceipts':[0,0,0,0,0,0,0,0,0,0],'projectedOnHand':[0,0,0,0,0,0,0,0,0,0],'netRequirements':[0,0,0,0,0,0,0,0,0,0],'plannedReceipt':[0,0,0,0,0,0,0,0,0,0],'plannedRelease':[0,0,0,0,0,0,0,0,0,0],'realizationTime':0,'lotSize':0,'BOM':0,'startingInventory':0,'autoPlanning':0}"




function sendData(){

    const xhr = new XMLHttpRequest()
    xhr.withCredentials = false;

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

    xhr.open('POST', 'https://20.113.171.243:8080/api/GetGHPTable', true)
    xhr.setRequestHeader('content-type', 'application/json')
    xhr.send(JSON.stringify(text))

    xhr.addEventListener('readystatechange', function () {
      if (this.readyState === this.DONE) {
        try{
          result = toCamel(JSON.parse(this.response))
        }catch{
          console.log(this.response)
          //Here will be code that will make popup with response message
        }

        const xhr = new XMLHttpRequest()
    xhr.withCredentials = false;
  xhr.open('POST', 'https://20.113.171.243:8080/api/GetMRPlvl1Table/'+document.getElementById("wTime").value+'/'+document.getElementById("wLotSize").value+'/1/'+document.getElementById("wInventory").value+'/true', true)
  xhr.setRequestHeader('content-type', 'application/json')
  result = JSON.stringify(result)
  
  console.log(JSON.stringify(result.replace(/["]/g,"'")))
  xhr.send(JSON.stringify(result.replace(/["]/g,"'")))

  xhr.addEventListener('readystatechange', function () {
    if (this.readyState === this.DONE) {

        console.log(JSON.parse(this.response))
        wMRP = JSON.parse(this.response)
        ShowResult()
       
    }
    })
        
    
        
      }
      })

    
  
  

  
}
  



function ShowResult() {

  
  result = JSON.parse(result)
  console.log(result)
  console.log(wMRP)
  for(var i =0; i<10; i++){
    if(result.salesForecast[i]!=0){
       document.getElementById("saleTable"+(i+1)).innerHTML = result.salesForecast[i]
    }else 
    {
      document.getElementById("saleTable"+(i+1)).innerHTML = "";
    }

    if(result.production[i]!=0){
      document.getElementById("productionTable"+(i+1)).innerHTML = result.production[i]
   }else 
   {
     document.getElementById("productionTable"+(i+1)).innerHTML = "";
   }
   
    document.getElementById("inventoryTable"+(i+1)).innerHTML = result.inventory[i]


    if(wMRP.GrossRequirements[i]!=0){
      document.getElementById("wGrossRequirements"+(i+1)).innerHTML = wMRP.GrossRequirements[i]
    }
    else {
     document.getElementById("wGrossRequirements"+(i+1)).innerHTML = "";
    }
  }
}

function toCamel(o) {
  var newO, origKey, newKey, value
  if (o instanceof Array) {
    return o.map(function(value) {
        if (typeof value === "object") {
          value = toCamel(value)
        }
        return value
    })
  } else {
    newO = {}
    for (origKey in o) {
      if (o.hasOwnProperty(origKey)) {
        newKey = (origKey.charAt(0).toLowerCase() + origKey.slice(1) || origKey).toString()
        value = o[origKey]
        if (value instanceof Array || (value !== null && value.constructor === Object)) {
          value = toCamel(value)
        }
        newO[newKey] = value
      }
    }
  }
  return newO
}
