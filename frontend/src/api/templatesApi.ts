export type TemplateDto = {
  id: number
  name: string
  htmlContent: string
  createdAt: string
}

export type CreateTemplateRequest = {
  name: string
  htmlContent: string
}

export type UpdateTemplateRequest = CreateTemplateRequest

// Use relative base so Vite dev server proxy forwards to backend
const API_BASE = ''

export async function getTemplates(): Promise<TemplateDto[]> {
  const res = await fetch(`${API_BASE}/api/Templates/GetAll`)
  if (!res.ok) throw new Error('Failed to load templates')
  return res.json()
}

export async function getTemplate(id: number): Promise<TemplateDto> {
  const res = await fetch(`${API_BASE}/api/Templates/GetById/${id}`)
  if (!res.ok) throw new Error('Template not found')
  return res.json()
}

export async function createTemplate(body: CreateTemplateRequest): Promise<TemplateDto> {
  const res = await fetch(`${API_BASE}/api/Templates/Create`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(body)
  })
  if (!res.ok) throw new Error('Failed to create template')
  return res.json()
}

export async function updateTemplate(id: number, body: UpdateTemplateRequest): Promise<TemplateDto> {
  const res = await fetch(`${API_BASE}/api/Templates/Update/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(body)
  })
  if (!res.ok) throw new Error('Failed to update template')
  return res.json()
}

export async function deleteTemplate(id: number): Promise<void> {
  const res = await fetch(`${API_BASE}/api/Templates/Delete/${id}`, { method: 'DELETE' })
  if (!res.ok) throw new Error('Failed to delete template')
}

export async function generatePdf(id: number, data: Record<string, string>): Promise<Blob> {
  const res = await fetch(`${API_BASE}/api/Templates/${id}/generate`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ data })
  })
  if (!res.ok) throw new Error('Failed to generate PDF')
  const blob = await res.blob()
  return blob
}
