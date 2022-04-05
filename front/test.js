//alert("aaa");

const ApiURL = "http://20.113.171.243"
var interval
var result
var wMRP = 0
var bMRP = 0
var cMRP = 0
var hMRP = 0
var text = "{'salesForecast':[0,0,0,0,10,0,20,0,0,0],'production':[0,0,0,0,0,0,0,0,0,0],'inventory':[0,0,0,0,0,0,0,0,0,0],'realizationTime':1,'startingInventory':2}";
var mrp = "{'grossRequirements':[0,0,0,0,10,0,20,0,0,0],'scheduledReceipts':[0,0,0,0,0,0,0,0,0,0],'projectedOnHand':[0,0,0,0,0,0,0,0,0,0],'netRequirements':[0,0,0,0,0,0,0,0,0,0],'plannedReceipt':[0,0,0,0,0,0,0,0,0,0],'plannedRelease':[0,0,0,0,0,0,0,0,0,0],'realizationTime':0,'lotSize':0,'BOM':0,'startingInventory':0,'autoPlanning':0}"

window.addEventListener("load", function () {
  setTimeout(() => {  const loading = document.querySelector(".loading");
  loading.className += " hidden"}, 2000);
});


function sendData(){
    const numInputs = document.querySelectorAll('input[type=number]')
    numInputs.forEach(changeValue) 
    
    function changeValue(input){
      if(input.value == ""){
        input.value = 0
      }
    }

    wMRP = 0
    bMRP = 0
    cMRP = 0
    hMRP = 0
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

    xhr.open('POST', ApiURL+'/api/GetGHPTable', true)
    xhr.setRequestHeader('content-type', 'application/json')
    xhr.send(JSON.stringify(text))

    xhr.addEventListener('readystatechange', function () {
      if (this.readyState === this.DONE) {
        try{
          result = toCamel(JSON.parse(this.response))
          
        
           //console.log(JSON.stringify(result.replace(/["]/g,"'")))
        }catch{
          console.log(this.response)
          //Here will be code that will make popup with response message
        }

        const xhrW = new XMLHttpRequest()
        xhrW.withCredentials = false;
        xhrW.open('POST', ApiURL+'/api/GetMRPlvl1Table/'+document.getElementById("wTime").value+'/'+document.getElementById("wLotSize").value+'/1/'+document.getElementById("wInventory").value+'/' + document.getElementById("auto").checked, true)
        xhrW.setRequestHeader('content-type', 'application/json')

        var wheelResult = result
        
        FillOrders("wheel", wheelResult)
        wheelResult = JSON.stringify(wheelResult)
        xhrW.send(JSON.stringify(wheelResult.replace(/["]/g,"'")))

        xhrW.addEventListener('readystatechange', function () {
          if (this.readyState === this.DONE) {
              console.log(JSON.parse(this.response))
              wMRP = JSON.parse(this.response)
              wMRP_ready = true
            
          }
        })  

        const xhrB = new XMLHttpRequest()
        xhrB.withCredentials = false;
        xhrB.open('POST', ApiURL+'/api/GetMRPlvl1Table/'+document.getElementById("bTime").value+'/'+document.getElementById("bLotSize").value+'/1/'+document.getElementById("bInventory").value+'/' + document.getElementById("auto").checked, true)
        xhrB.setRequestHeader('content-type', 'application/json')

        var boxResult = result

        FillOrders("box", boxResult)
        boxResult = JSON.stringify(boxResult)
        xhrB.send(JSON.stringify(boxResult.replace(/["]/g,"'")))

        xhrB.addEventListener('readystatechange', function () {
          if (this.readyState === this.DONE) {
              console.log(JSON.parse(this.response))
              bMRP = JSON.parse(this.response)
              bMRP_ready = true
            
          }
        }) 

        const xhrC = new XMLHttpRequest()
        xhrC.withCredentials = false;
        xhrC.open('POST', ApiURL+'/api/GetMRPlvl1Table/'+document.getElementById("cTime").value+'/'+document.getElementById("cLotSize").value+'/1/'+document.getElementById("cInventory").value+'/' + document.getElementById("auto").checked, true)
        xhrC.setRequestHeader('content-type', 'application/json')

        var cageResult = result
        FillOrders("cage", cageResult)
        cageResult = JSON.stringify(cageResult)
        xhrC.send(JSON.stringify(cageResult.replace(/["]/g,"'")))

        xhrC.addEventListener('readystatechange', function () {
          if (this.readyState === this.DONE) {
              console.log(JSON.parse(this.response))
              cMRP = JSON.parse(this.response)
              cMRP_ready = true

              const xhrH = new XMLHttpRequest()
              xhrH.withCredentials = false;
              xhrH.open('POST', ApiURL+'/api/GetMRPlvl2Table/'+document.getElementById("hTime").value+'/'+document.getElementById("hLotSize").value+'/2/'+document.getElementById("hInventory").value+'/' + document.getElementById("auto").checked, true)
              xhrH.setRequestHeader('content-type', 'application/json')
              var handlesResult = cMRP
              FillOrders("handles", handlesResult)
              var mrp2 = '"'+(JSON.stringify(handlesResult)).replace(/["]/g,"'")+'"'
              xhrH.send(mrp2)

              xhrH.addEventListener('readystatechange', function () {
                if (this.readyState === this.DONE) {
                    console.log(JSON.parse(this.response))
                    hMRP = JSON.parse(this.response)
                    hMRP_ready = true
            
                }
              })
          }
        }) 
      }
    })

    interval = setInterval(ShowResult, 100)
  
}
  



function ShowResult() {

  if(wMRP != 0 && bMRP != 0 && cMRP != 0 && hMRP != 0){
    clearInterval(interval)
    var tableLetters = ["w", "b", "c", "h"]
    var tableRows = ["GrossRequirements", "NetRequirements", "PlannedReceipt", "PlannedRelease", "ProjectedOnHand", "SheduledReceipts"]
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

      tableLetters.forEach(function(letter){
        tableRows.forEach(function(row){
          eval('var tableMRP = ' + letter + 'MRP.' + row)
          if(row == "SheduledReceipts"){
            if(tableMRP[i]!=0){
              document.getElementById(letter + row +(i+1)).value = tableMRP[i]
            }
            else {
            document.getElementById(letter + row +(i+1)).value = 0;
            }
          }
          else{
            if(tableMRP[i]!=0){
              document.getElementById(letter + row +(i+1)).innerHTML = tableMRP[i]
            }
            else {
              if(row = "ProjectedOnHand"){
                document.getElementById(letter + row +(i+1)).innerHTML = "0";
              }
              else{
                document.getElementById(letter + row +(i+1)).innerHTML = "";
              }
              
            }
          }
          
        })
      })
    }
  }
}

function FillOrders(name, json){
  switch(name){
    case "wheel":
      for(var i = 0; i < 10; i++){
        json.orders[i] = document.getElementById("wSheduledReceipts"+(i+1)).value
        if(document.getElementById("wSheduledReceipts"+(i+1)).value == ''){
          json.orders[i] = 0
        }
      }
      break;
    case "box":
      for(var i = 0; i < 10; i++){
        json.orders[i] = document.getElementById("bSheduledReceipts"+(i+1)).value
        if(document.getElementById("bSheduledReceipts"+(i+1)).value == ''){
          json.orders[i] = 0
        }
      }
      break;
    case "cage":
      for(var i = 0; i < 10; i++){
        json.orders[i] = document.getElementById("cSheduledReceipts"+(i+1)).value
        if(document.getElementById("cSheduledReceipts"+(i+1)).value == ''){
          json.orders[i] = 0
        }
      }
      break;
    case "handles":
      for(var i = 0; i < 10; i++){
        json.Orders[i] = document.getElementById("hSheduledReceipts"+(i+1)).value
        if(document.getElementById("hSheduledReceipts"+(i+1)).value == ''){
          json.Orders[i] = 0
        }
      }
      break;
  }
}

// Checkbox table cell toggle
$(function() {
  $('td').click(function() {
    $(this).find(':checkbox').click();
  });

  $(":checkbox").click(function(e) {
    e.stopPropagation();
  });
});

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
