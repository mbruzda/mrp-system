//alert("aaa");
var interval
var result
var wMRP = 0
var bMRP = 0
var wMRP_ready = false;
var bMRP_ready = false;
var text = "{'salesForecast':[0,0,0,0,10,0,20,0,0,0],'production':[0,0,0,0,0,0,0,0,0,0],'inventory':[0,0,0,0,0,0,0,0,0,0],'realizationTime':1,'startingInventory':2}";
var mrp = "{'grossRequirements':[0,0,0,0,10,0,20,0,0,0],'scheduledReceipts':[0,0,0,0,0,0,0,0,0,0],'projectedOnHand':[0,0,0,0,0,0,0,0,0,0],'netRequirements':[0,0,0,0,0,0,0,0,0,0],'plannedReceipt':[0,0,0,0,0,0,0,0,0,0],'plannedRelease':[0,0,0,0,0,0,0,0,0,0],'realizationTime':0,'lotSize':0,'BOM':0,'startingInventory':0,'autoPlanning':0}"




function sendData(){

    wMRP = 0
    bMRP = 0
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

    xhr.open('POST', 'http://erp.oskarkozaczka.pl/api/GetGHPTable', true)
    xhr.setRequestHeader('content-type', 'application/json')
    xhr.send(JSON.stringify(text))

    xhr.addEventListener('readystatechange', function () {
      if (this.readyState === this.DONE) {
        try{
          result = toCamel(JSON.parse(this.response))
          result = JSON.stringify(result)
        
          console.log(JSON.stringify(result.replace(/["]/g,"'")))
        }catch{
          console.log(this.response)
          //Here will be code that will make popup with response message
        }

        const xhr = new XMLHttpRequest()
    xhr.withCredentials = false;
  xhr.open('POST', 'http://erp.oskarkozaczka.pl/api/GetMRPlvl1Table/'+document.getElementById("wTime").value+'/'+document.getElementById("wLotSize").value+'/1/'+document.getElementById("wInventory").value+'/true', true)
  xhr.setRequestHeader('content-type', 'application/json')
  result = JSON.stringify(result)
  
  console.log(JSON.stringify(result.replace(/["]/g,"'")))
  xhr.send(JSON.stringify(result.replace(/["]/g,"'")))

        xhrW.addEventListener('readystatechange', function () {
          if (this.readyState === this.DONE) {
              console.log(JSON.parse(this.response))
              wMRP = JSON.parse(this.response)
              wMRP_ready = true
            
          }
        })  

        const xhrB = new XMLHttpRequest()
        xhrB.withCredentials = false;
        xhrB.open('POST', 'http://35.246.143.214/api/GetMRPlvl1Table/'+document.getElementById("bTime").value+'/'+document.getElementById("bLotSize").value+'/1/'+document.getElementById("bInventory").value+'/true', true)
        xhrB.setRequestHeader('content-type', 'application/json')

        xhrB.send(JSON.stringify(result.replace(/["]/g,"'")))

        xhrB.addEventListener('readystatechange', function () {
          if (this.readyState === this.DONE) {
              console.log(JSON.parse(this.response))
              bMRP = JSON.parse(this.response)
              bMRP_ready = true
            
          }
        }) 
      }
    })

    interval = setInterval(ShowResult, 100)
  
}
  



function ShowResult() {

  if(wMRP != 0 && bMRP != 0){
    clearInterval(interval)
    result = JSON.parse(result)
    console.log(result)
    console.log(wMRP)
    console.log(bMRP)
    for(var i =0; i<10; i++){
      if(result.salesForecast[i]!=0){
        document.getElementById("saleTable"+(i+1)).innerHTML = result.salesForecast[i]
      }
      else 
      {
        document.getElementById("saleTable"+(i+1)).innerHTML = "";
      }

      if(result.production[i]!=0){
        document.getElementById("productionTable"+(i+1)).innerHTML = result.production[i]
      }
      else {
      document.getElementById("productionTable"+(i+1)).innerHTML = "";
      }
      document.getElementById("inventoryTable"+(i+1)).innerHTML = result.inventory[i]


      if(wMRP.GrossRequirements[i]!=0){
        document.getElementById("wGrossRequirements"+(i+1)).innerHTML = wMRP.GrossRequirements[i]
      }
      else {
      document.getElementById("wGrossRequirements"+(i+1)).innerHTML = "";
      }
      if(wMRP.NetRequirements[i]!=0){
        document.getElementById("wNetRequirements"+(i+1)).innerHTML = wMRP.NetRequirements[i]
      }
      else {
      document.getElementById("wNetRequirements"+(i+1)).innerHTML = "";
      }
      if(wMRP.PlannedReceipt[i]!=0){
        document.getElementById("wPlannedReceipts"+(i+1)).innerHTML = wMRP.PlannedReceipt[i]
      }
      else {
      document.getElementById("wPlannedReceipts"+(i+1)).innerHTML = "";
      }
      if(wMRP.PlannedRelease[i]!=0){
        document.getElementById("wPlannedRelease"+(i+1)).innerHTML = wMRP.PlannedRelease[i]
      }
      else {
      document.getElementById("wPlannedRelease"+(i+1)).innerHTML = "";
      }
      if(wMRP.ProjectedOnHand[i]!=0){
        document.getElementById("wProjectedOnHand"+(i+1)).innerHTML = wMRP.ProjectedOnHand[i]
      }
      else {
      document.getElementById("wProjectedOnHand"+(i+1)).innerHTML = "";
      }
      if(wMRP.SheduledReceipts[i]!=0){
        document.getElementById("wScheduledReceipts"+(i+1)).innerHTML = wMRP.SheduledReceipts[i]
      }
      else {
      document.getElementById("wScheduledReceipts"+(i+1)).innerHTML = "";
      }


      if(bMRP.GrossRequirements[i]!=0){
        document.getElementById("bGrossRequirements"+(i+1)).innerHTML = bMRP.GrossRequirements[i]
      }
      else {
      document.getElementById("bGrossRequirements"+(i+1)).innerHTML = "";
      }
      if(bMRP.NetRequirements[i]!=0){
        document.getElementById("bNetRequirements"+(i+1)).innerHTML = bMRP.NetRequirements[i]
      }
      else {
      document.getElementById("bNetRequirements"+(i+1)).innerHTML = "";
      }
      if(bMRP.PlannedReceipt[i]!=0){
        document.getElementById("bPlannedReceipts"+(i+1)).innerHTML = bMRP.PlannedReceipt[i]
      }
      else {
      document.getElementById("bPlannedReceipts"+(i+1)).innerHTML = "";
      }
      if(bMRP.PlannedRelease[i]!=0){
        document.getElementById("bPlannedRelease"+(i+1)).innerHTML = bMRP.PlannedRelease[i]
      }
      else {
      document.getElementById("bPlannedRelease"+(i+1)).innerHTML = "";
      }
      if(bMRP.ProjectedOnHand[i]!=0){
        document.getElementById("bProjectedOnHand"+(i+1)).innerHTML = bMRP.ProjectedOnHand[i]
      }
      else {
      document.getElementById("bProjectedOnHand"+(i+1)).innerHTML = "";
      }
      if(bMRP.SheduledReceipts[i]!=0){
        document.getElementById("bScheduledReceipts"+(i+1)).innerHTML = bMRP.SheduledReceipts[i]
      }
      else {
      document.getElementById("bScheduledReceipts"+(i+1)).innerHTML = "";
      }
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
