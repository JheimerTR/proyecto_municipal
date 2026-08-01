const apiUrl = 'https://localhost:7105/api/Usuario';

document.addEventListener("DOMContentLoaded", function () {
    CargarUsuarios();

    const formulario = document.getElementById("frmUsuario");
    if (formulario) {
        formulario.addEventListener("submit", function (event) {
            event.preventDefault();
            Guardar();
        });
    }
});

function CargarUsuarios() {
    fetch(apiUrl)
        .then(respuesta => respuesta.json())
        .then(datos => {
            let filas = '';
            datos.forEach(usuario => {
                filas += `
                    <tr>
                        <td>${usuario.usuarioID || usuario.id}</td>
                        <td>${usuario.usuarioRed}</td>
                        <td>${usuario.nombreCompleto}</td>
                        <td>${usuario.rol}</td>
                        <td>${usuario.activo === 1 || usuario.activo === true ? 'Sí' : 'No'}</td>
                        <td>
                            <button class="btn btn-sm btn-primary" type="button" onclick="CargarParaEditar(${usuario.usuarioID || usuario.id}, '${usuario.usuarioRed}', '${usuario.contrasena}', '${usuario.nombreCompleto}', '${usuario.rol}')">Editar</button>
                            <button class="btn btn-sm btn-danger" type="button" onclick="EliminarUsuario(${usuario.usuarioID || usuario.id})">Eliminar</button>
                        </td>
                    </tr>
                `;
            });
            document.getElementById('tbUsuarios').innerHTML = filas;
        })
        .catch(error => console.error(error));
}

function CargarParaEditar(id, usuarioRed, contrasena, nombre, rol) {
    document.getElementById('txtUsuarioID').value = id;
    document.getElementById('txtUsuarioRed').value = usuarioRed;
    document.getElementById('txtContrasena').value = contrasena || "";
    document.getElementById('txtNombreCompleto').value = nombre;
    document.getElementById('cboRol').value = rol;
}

function limpiarFormulario() {
    document.getElementById('txtUsuarioID').value = "0";
    document.getElementById('txtUsuarioRed').value = "";
    document.getElementById('txtContrasena').value = "";
    document.getElementById('txtNombreCompleto').value = "";
    document.getElementById('cboRol').value = "";
}

function Guardar() {
    const id = document.getElementById('txtUsuarioID').value;

    const objetoData = {
        usuarioID: id == "0" ? 0 : parseInt(id),
        usuarioRed: document.getElementById('txtUsuarioRed').value,
        contrasena: document.getElementById('txtContrasena').value,
        nombreCompleto: document.getElementById('txtNombreCompleto').value,
        rol: document.getElementById('cboRol').value,
        activo: parseInt(document.getElementById('cboActivo').value)
    };

    const metodo = id == "0" ? 'POST' : 'PUT';
    const urlDestino = id == "0" ? apiUrl : `${apiUrl}/${id}`;

    fetch(urlDestino, {
        method: metodo,
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(objetoData)
    })
        .then(respuesta => {
            if (respuesta.ok) {
                limpiarFormulario();
                CargarUsuarios();
            }
        })
        .catch(error => console.error(error));
}

function EliminarUsuario(id) {
    if (confirm("¿Seguro de eliminar?")) {
        fetch(`${apiUrl}/${id}`, {
            method: 'DELETE'
        })
            .then(respuesta => {
                if (respuesta.ok) {
                    CargarUsuarios();
                }
            });
    }
}