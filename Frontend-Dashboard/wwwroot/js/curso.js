const apiUrl = 'https://localhost:7105/api/Curso';

function CargarDatos() {
    fetch(apiUrl)
        .then(res => res.json())
        .then(datos => {
            let filas = '';
            datos.forEach(item => {
                // Manejo seguro por si C# devuelve los datos con Mayúsculas
                const id = item.cursoID || item.CursoID || item.id || item.Id;
                const nombre = item.nombre || item.Nombre || '';
                const categoria = item.categoria || item.Categoria || '';
                const cupoMaximo = item.cupoMaximo || item.CupoMaximo || 0;
                const instructorID = item.instructorID || item.InstructorID || 0;
                const espacioID = item.espacioID || item.EspacioID || 0;
                const horarioID = item.horarioID || item.HorarioID || 0;

                filas += `
                    <tr>
                        <td>${id}</td>
                        <td>${nombre}</td>
                        <td>${categoria}</td>
                        <td>${cupoMaximo}</td>
                        <td>${instructorID}</td>
                        <td>${espacioID}</td>
                        <td>${horarioID}</td>
                        <td>
                            <button type="button" onclick="CargarParaEditar(${id}, '${nombre}', '${categoria}', ${cupoMaximo}, ${instructorID}, ${espacioID}, ${horarioID})">Editar</button>
                            <button type="button" onclick="Eliminar(${id})">Eliminar</button>
                        </td>
                    </tr>`;
            });
            document.getElementById('tabla-datos').innerHTML = filas;
        })
        .catch(err => console.error('Error:', err));
}

function CargarParaEditar(id, nombre, categoria, cupo, instructorId, espacioId, horarioId) {
    document.getElementById('txtId').value = id;
    document.getElementById('txtNombre').value = nombre;
    document.getElementById('txtCategoria').value = categoria;
    document.getElementById('txtCupoMaximo').value = cupo;
    document.getElementById('txtInstructorID').value = instructorId;
    document.getElementById('txtEspacioID').value = espacioId;
    document.getElementById('txtHorarioID').value = horarioId;
}

function LimpiarFormulario() {
    document.getElementById('txtId').value = "0";
    document.getElementById('txtNombre').value = "";
    document.getElementById('txtCategoria').value = "";
    document.getElementById('txtCupoMaximo').value = "";
    document.getElementById('txtInstructorID').value = "";
    document.getElementById('txtEspacioID').value = "";
    document.getElementById('txtHorarioID').value = "";
}

// Añadimos 'event' para evitar que la página se recargue
function Guardar(event) {
    if (event) event.preventDefault();

    // 1. Capturamos los elementos para asegurar que existen
    const elId = document.getElementById('txtId');
    const elNombre = document.getElementById('txtNombre');
    const elCategoria = document.getElementById('txtCategoria');
    const elCupoMaximo = document.getElementById('txtCupoMaximo');
    const elInstructorID = document.getElementById('txtInstructorID');
    const elEspacioID = document.getElementById('txtEspacioID');
    const elHorarioID = document.getElementById('txtHorarioID');

    // 2. Verificamos que no haya errores de escritura en tu HTML
    if (!elId) { alert("Error: No se encontró el input 'txtId'"); return; }
    if (!elNombre) { alert("Error: No se encontró el input 'txtNombre'"); return; }
    if (!elCategoria) { alert("Error: No se encontró el input 'txtCategoria'"); return; }
    if (!elCupoMaximo) { alert("Error: No se encontró el input 'txtCupoMaximo'"); return; }
    if (!elInstructorID) { alert("Error: No se encontró el input 'txtInstructorID'"); return; }
    if (!elEspacioID) { alert("Error: No se encontró el input 'txtEspacioID'"); return; }
    if (!elHorarioID) { alert("Error: No se encontró el input 'txtHorarioID'"); return; }

    const id = elId.value;

    const objetoData = {
        cursoID: id === "0" ? 0 : parseInt(id),
        nombre: elNombre.value,
        categoria: elCategoria.value,
        cupoMaximo: parseInt(elCupoMaximo.value),
        instructorID: parseInt(elInstructorID.value),
        espacioID: parseInt(elEspacioID.value),
        horarioID: parseInt(elHorarioID.value)
    };

    fetch(id === "0" ? apiUrl : `${apiUrl}/${id}`, {
        method: id === "0" ? 'POST' : 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(objetoData)
    })
        .then(async res => {
            if (res.ok) {
                alert(id === "0" ? "Curso creado exitosamente." : "Curso actualizado correctamente.");
                LimpiarFormulario();
                CargarDatos();
            } else {
                const data = await res.json().catch(() => null);
                alert("Atención: " + (data ? data.error : "Error en base de datos (Ej: Llave foránea incorrecta)"));
            }
        })
        .catch(err => {
            alert("Ocurrió un error de conexión con el servidor.");
            console.error('Error:', err);
        });
}

function Eliminar(id) {
    if (!confirm("¿Estás seguro de que deseas eliminar este curso?")) return;

    fetch(`${apiUrl}/${id}`, { method: 'DELETE' })
        .then(res => { if (res.ok) CargarDatos(); })
        .catch(err => console.error('Error:', err));
}

CargarDatos();