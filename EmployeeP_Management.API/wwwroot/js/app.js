// app.js - Employee Management Frontend Logic using backend API

// ── Toast notification helper ──────────────────────────
function showToast(message, type = 'danger') {
  const toastEl = document.getElementById('appToast');
  const toastBody = document.getElementById('toastBody');
  // Set color class
  toastEl.className = `toast align-items-center border-0 text-bg-${type}`;
  toastBody.innerHTML = message;
  const toast = bootstrap.Toast.getOrCreateInstance(toastEl, { delay: 5000 });
  toast.show();
}

// ── Parse server error into friendly message ───────────
function parseApiError(errorText) {
  let msg = errorText;

  // The ExceptionMiddleware returns JSON: { "statusCode": ..., "message": "..." }
  try {
    const parsed = JSON.parse(errorText);
    if (parsed.message) msg = parsed.message;
    else if (parsed.Message) msg = parsed.Message;
    else if (parsed.title) msg = parsed.title;
    else if (parsed.errors) {
      const msgs = Object.values(parsed.errors).flat();
      return '<i class="bi bi-exclamation-triangle me-2"></i>' + msgs.join('<br>');
    }
  } catch (_) {
    // Not JSON — use raw text
  }

  const lower = msg.toLowerCase();

  // Exact matches from EmployeeService
  if (lower.includes('email already exists')) {
    return '<i class="bi bi-envelope-exclamation me-2"></i>This email address is already in use. Please use a different email.';
  }
  if (lower.includes('mobile number already exists')) {
    return '<i class="bi bi-phone me-2"></i>This mobile number is already in use. Please use a different number.';
  }
  if (lower.includes('not found')) {
    return '<i class="bi bi-search me-2"></i>' + msg;
  }

  return '<i class="bi bi-exclamation-circle me-2"></i>' + msg;
}

// ── API request helper ─────────────────────────────────
async function apiRequest(url, method = 'GET', data = null) {
  const options = {
    method,
    headers: {
      'Content-Type': 'application/json',
    },
  };
  if (data) {
    options.body = JSON.stringify(data);
  }
  const response = await fetch(url, options);
  if (!response.ok) {
    const err = await response.text();
    throw new Error(err || `Request failed with status ${response.status}`);
  }
  // Some actions (Create, Delete, Update) may return empty body
  if (response.status === 204) return null;
  const text = await response.text();
  if (!text) return null;
  return JSON.parse(text);
}

// ── Status icon helper ─────────────────────────────────
function statusIcon(isActive) {
  if (isActive) {
    return '<i class="bi bi-check-circle-fill status-active fs-5" title="Active"></i>';
  }
  return '<i class="bi bi-x-circle-fill status-inactive fs-5" title="Inactive"></i>';
}

// ── Employee row HTML helper ───────────────────────────
function employeeRowHtml(emp) {
  const hireDate = emp.hireDate ? new Date(emp.hireDate).toLocaleDateString() : '';
  return `
    <td>${emp.fullName}</td>
    <td>${emp.email}</td>
    <td>${emp.mobileNumber || ''}</td>
    <td>${emp.jobTitle || ''}</td>
    <td>${emp.departmentName}</td>
    <td>${hireDate}</td>
    <td class="text-center">${statusIcon(emp.isActive)}</td>
    <td>
      <button class="btn btn-sm btn-primary me-1" onclick="openEditEmployee(${emp.id})">
        <i class="bi bi-pencil-square"></i> Edit
      </button>
      <button class="btn btn-sm btn-danger" onclick="deleteEmployee(${emp.id})">
        <i class="bi bi-trash3"></i> Delete
      </button>
    </td>`;
}

/* ---------- Employees ---------- */
let employeesCache = [];

async function loadEmployees() {
  employeesCache = await apiRequest('/api/Employees');
  renderEmployees();
}

function getFilteredByStatus(list) {
  const filter = document.getElementById('statusFilter')?.value || 'all';
  if (filter === 'active') return list.filter(e => e.isActive);
  if (filter === 'inactive') return list.filter(e => !e.isActive);
  return list;
}

function renderEmployees() {
  const tbody = document.getElementById('employeesBody');
  tbody.innerHTML = '';
  const filtered = getFilteredByStatus(employeesCache);
  filtered.forEach((emp) => {
    const tr = document.createElement('tr');
    tr.innerHTML = employeeRowHtml(emp);
    tbody.appendChild(tr);
  });
}

let searchTimeout = null;
function filterEmployees() {
  const query = document.getElementById('searchInput').value.trim();
  clearTimeout(searchTimeout);
  if (!query) {
    renderEmployees();
    return;
  }
  searchTimeout = setTimeout(async () => {
    try {
      const results = await apiRequest(`/api/Employees/search?searchTerm=${encodeURIComponent(query)}`);
      const tbody = document.getElementById('employeesBody');
      tbody.innerHTML = '';
      results.forEach((emp) => {
        const tr = document.createElement('tr');
        tr.innerHTML = employeeRowHtml(emp);
        tbody.appendChild(tr);
      });
    } catch (e) {
      console.error('Search failed:', e);
    }
  }, 300);
}

async function openAddEmployee() {
  document.getElementById('employeeModalLabel').innerText = 'Add Employee';
  document.getElementById('empName').value = '';
  document.getElementById('empEmail').value = '';
  document.getElementById('empMobile').value = '';
  document.getElementById('empJobTitle').value = '';
  document.getElementById('empHireDate').value = new Date().toISOString().split('T')[0];
  document.getElementById('empIsActive').checked = true;
  await populateDepartmentDropdown();
  document.getElementById('empIndex').value = '';
}

async function populateDepartmentDropdown(selectedId = '') {
  const deptSelect = document.getElementById('empDept');
  const departments = await apiRequest('/api/Departments');
  deptSelect.innerHTML = '<option value="" disabled>Select department</option>';
  departments.forEach(d => {
    const opt = document.createElement('option');
    opt.value = d.id;
    opt.textContent = d.name;
    if (d.id == selectedId) opt.selected = true;
    deptSelect.appendChild(opt);
  });
}

async function openEditEmployee(id) {
  const emp = employeesCache.find(e => e.id === id);
  if (!emp) return showToast('Employee not found', 'warning');
  document.getElementById('employeeModalLabel').innerText = 'Edit Employee';
  document.getElementById('empName').value = emp.fullName;
  document.getElementById('empEmail').value = emp.email;
  document.getElementById('empMobile').value = emp.mobileNumber || '';
  document.getElementById('empJobTitle').value = emp.jobTitle || '';
  document.getElementById('empHireDate').value = emp.hireDate ? emp.hireDate.split('T')[0] : '';
  document.getElementById('empIsActive').checked = emp.isActive;
  await populateDepartmentDropdown(emp.departmentId);
  document.getElementById('empIndex').value = id;
  const modal = new bootstrap.Modal(document.getElementById('employeeModal'));
  modal.show();
}

async function saveEmployee() {
  const fullName = document.getElementById('empName').value.trim();
  const email = document.getElementById('empEmail').value.trim();
  const mobileNumber = document.getElementById('empMobile').value.trim();
  const jobTitle = document.getElementById('empJobTitle').value.trim();
  const departmentId = document.getElementById('empDept').value;
  const hireDate = document.getElementById('empHireDate').value;
  const isActive = document.getElementById('empIsActive').checked;
  const id = document.getElementById('empIndex').value;

  // ── Client-side validation (mirrors backend DTO rules) ──
  if (!fullName || !email || !mobileNumber || !jobTitle || !departmentId || !hireDate) {
    return showToast('<i class="bi bi-info-circle me-2"></i>Please fill all required fields.', 'warning');
  }
  // [EmailAddress] validation
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  if (!emailRegex.test(email)) {
    return showToast('<i class="bi bi-envelope-exclamation me-2"></i>Please enter a valid email address.', 'warning');
  }
  // MobileNumber: digits only, 10-15 chars (international phone format)
  const phoneRegex = /^\+?[0-9]{10,15}$/;
  if (!phoneRegex.test(mobileNumber)) {
    return showToast('<i class="bi bi-phone me-2"></i>Please enter a valid mobile number (10-15 digits).', 'warning');
  }
  // FullName: at least 2 characters
  if (fullName.length < 2) {
    return showToast('<i class="bi bi-person-exclamation me-2"></i>Full name must be at least 2 characters.', 'warning');
  }

  const payload = { fullName, email, mobileNumber, jobTitle, departmentId: Number(departmentId), hireDate, isActive };
  try {
    if (id) {
      await apiRequest(`/api/Employees/${id}`, 'PUT', payload);
      showToast('<i class="bi bi-check-lg me-2"></i>Employee updated successfully!', 'success');
    } else {
      await apiRequest('/api/Employees', 'POST', payload);
      showToast('<i class="bi bi-check-lg me-2"></i>Employee added successfully!', 'success');
    }
    await loadEmployees();
    const modalEl = document.getElementById('employeeModal');
    const modal = bootstrap.Modal.getInstance(modalEl);
    if (modal) modal.hide();
  } catch (e) {
    showToast(parseApiError(e.message), 'danger');
  }
}

async function deleteEmployee(id) {
  if (!confirm('Delete this employee?')) return;
  try {
    await apiRequest(`/api/Employees/${id}`, 'DELETE');
    showToast('<i class="bi bi-check-lg me-2"></i>Employee deleted.', 'success');
    await loadEmployees();
  } catch (e) {
    showToast(parseApiError(e.message), 'danger');
  }
}

/* ---------- Departments ---------- */
let departmentsCache = [];

async function loadDepartments() {
  departmentsCache = await apiRequest('/api/Departments');
  renderDepartments();
}

function renderDepartments() {
  const tbody = document.getElementById('departmentsBody');
  tbody.innerHTML = '';
  departmentsCache.forEach((dept) => {
    const tr = document.createElement('tr');
    tr.innerHTML = `
      <td>${dept.name}</td>
      <td>
        <button class="btn btn-sm btn-primary me-1" onclick="openEditDepartment(${dept.id})">
          <i class="bi bi-pencil-square"></i> Edit
        </button>
        <button class="btn btn-sm btn-danger" onclick="deleteDepartment(${dept.id})">
          <i class="bi bi-trash3"></i> Delete
        </button>
      </td>`;
    tbody.appendChild(tr);
  });
}

async function openAddDepartment() {
  document.getElementById('departmentModalLabel').innerText = 'Add Department';
  document.getElementById('deptName').value = '';
  document.getElementById('deptIndex').value = '';
}

async function openEditDepartment(id) {
  const dept = departmentsCache.find(d => d.id === id);
  if (!dept) return showToast('Department not found', 'warning');
  document.getElementById('departmentModalLabel').innerText = 'Edit Department';
  document.getElementById('deptName').value = dept.name;
  document.getElementById('deptIndex').value = id;
  const modal = new bootstrap.Modal(document.getElementById('departmentModal'));
  modal.show();
}

async function saveDepartment() {
  const name = document.getElementById('deptName').value.trim();
  // ── Client-side validation (mirrors backend DTO rules) ──
  if (!name) {
    return showToast('<i class="bi bi-info-circle me-2"></i>Please enter a department name.', 'warning');
  }
  // [MaxLength(100)] validation
  if (name.length > 100) {
    return showToast('<i class="bi bi-exclamation-triangle me-2"></i>Department name must be 100 characters or less.', 'warning');
  }
  // At least 2 characters
  if (name.length < 2) {
    return showToast('<i class="bi bi-exclamation-triangle me-2"></i>Department name must be at least 2 characters.', 'warning');
  }
  const id = document.getElementById('deptIndex').value;
  const payload = { name };
  try {
    if (id) {
      await apiRequest(`/api/Departments/${id}`, 'PUT', payload);
      showToast('<i class="bi bi-check-lg me-2"></i>Department updated!', 'success');
    } else {
      await apiRequest('/api/Departments', 'POST', payload);
      showToast('<i class="bi bi-check-lg me-2"></i>Department added!', 'success');
    }
    await loadDepartments();
    const modalEl = document.getElementById('departmentModal');
    const modal = bootstrap.Modal.getInstance(modalEl);
    if (modal) modal.hide();
  } catch (e) {
    showToast(parseApiError(e.message), 'danger');
  }
}

async function deleteDepartment(id) {
  if (!confirm('Delete this department?')) return;
  try {
    await apiRequest(`/api/Departments/${id}`, 'DELETE');
    showToast('<i class="bi bi-check-lg me-2"></i>Department deleted.', 'success');
    await loadDepartments();
  } catch (e) {
    showToast(parseApiError(e.message), 'danger');
  }
}

/* ---------- Navigation ---------- */
function showSection(section) {
  if (section === 'employee') {
    document.getElementById('employeeSection').style.display = '';
    document.getElementById('departmentSection').style.display = 'none';
  } else {
    document.getElementById('employeeSection').style.display = 'none';
    document.getElementById('departmentSection').style.display = '';
  }
}

/* ---------- Initialization ---------- */
async function initApp() {
  await Promise.all([loadEmployees(), loadDepartments()]);
}

initApp();

// Expose functions for HTML inline handlers
window.filterEmployees = filterEmployees;
window.openAddEmployee = openAddEmployee;
window.openEditEmployee = openEditEmployee;
window.saveEmployee = saveEmployee;
window.deleteEmployee = deleteEmployee;
window.openAddDepartment = openAddDepartment;
window.openEditDepartment = openEditDepartment;
window.saveDepartment = saveDepartment;
window.deleteDepartment = deleteDepartment;
window.showSection = showSection;
window.applyStatusFilter = function () { renderEmployees(); };
