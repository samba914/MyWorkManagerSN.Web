const compare = (ids, asc) => (row1, row2) => {
      const tdValue = (row, ids) => row.children[ids].textContent;
      const tri = (v1, v2) => v1 !== '' && v2 !== '' && !isNaN(v1) && !isNaN(v2) ? v1 - v2 : v1.toString().localeCompare(v2);


      var type= row1.children[ids].getAttribute("data-type");
      if(type=="amount"){
          var a = tdValue(asc ? row1 : row2, ids).split(" ")[0].replace(",", ".");;
          var b = tdValue(asc ? row2 : row1, ids).split(" ")[0].replace(",", ".");;
          return parseFloat(a) - parseFloat(b);
      }else{
          return tri(tdValue(asc ? row1 : row2, ids), tdValue(asc ? row2 : row1, ids));
      }


    };

    const tbody = document.querySelector('tbody');
    const thx = document.querySelectorAll('th');
    const trxb = tbody.querySelectorAll('tr');
    thx.forEach(th => th.addEventListener('click', () => {
      $("#arrawDownSort").remove();
      $("#arrawUpSort").remove();
      if(!this.asc){
          $(th).append("<i id='arrawDownSort' class='fa fa-arrow-down ml-1' aria-hidden='true'></i>");

      }else{
           $(th).append("<i id='arrawUpSort' class='fa fa-arrow-up ml-1' aria-hidden='true'></i>");
      }


      let classe = Array.from(trxb).sort(compare(Array.from(thx).indexOf(th), this.asc = !this.asc));
      classe.forEach(tr => tbody.appendChild(tr));
    }));