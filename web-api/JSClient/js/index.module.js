/* old method, change type of script in html
async function run() {
    const res = await fetch('https://localhost:7096/users');
    const data = await res.json();
    alert(data[0].fullName);
}
run();
*/
const apiUrl = "https://localhost:7096/users";
async function refreshList() {
  try {
    const res = await fetch(apiUrl);
    const data = await res.json();
    //alert(data[0].fullName);
    let trs = "";
    for (const user of data) {
      //alert(user.fullName);
      trs += `<tr>
        <td>${user.fullName}</td>
        <td>${user.email}</td>
        </tr>`;
    }
    document.querySelector("#tbody").innerHTML = trs;
  } catch (err) {
    alert("Error: " + err.message);
  }
}
const debug = true;
if (debug) {
  document.querySelector("#name").value = "Mirit";
  document.querySelector("#email").value = "mirit@gmail.com";
}

const btn = document.querySelector("#btnAdd");
btn.addEventListener("click", async function () {
  const name = document.querySelector("#name").value;
  const email = document.querySelector("#email").value;
  //validation here
  const payload = {
    fullName: name,
    email: email,
  };
  const req = {
    method: "POST",
    headers: {
      "Content-Type": "application/json", //MIME
    },
    body: JSON.stringify(payload),
  };
  const res = await fetch(apiUrl, req);
  const result = await res.json();
  alert(result);
  await refreshList();
});
await refreshList();
//AJAX async javascript and xml
