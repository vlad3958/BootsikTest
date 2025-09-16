import React, { useEffect, useMemo, useState } from 'react'
import { createTemplate, deleteTemplate, generatePdf, getTemplates, TemplateDto, updateTemplate } from '../api/templatesApi'

export function TemplatesPage() {
  const [items, setItems] = useState<TemplateDto[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const [editing, setEditing] = useState<TemplateDto | null>(null)
  const [name, setName] = useState('')
  const [html, setHtml] = useState('')

  const [jsonInput, setJsonInput] = useState('')

  useEffect(() => {
    (async () => {
      try {
        const data = await getTemplates()
        setItems(data)
      } catch (e: any) {
        setError(e.message)
      } finally { setLoading(false) }
    })()
  }, [])

  function resetForm() {
    setEditing(null)
    setName('')
    setHtml('')
  }

  async function onSubmit(e: React.FormEvent) {
    e.preventDefault()
    try {
      if (editing) {
        const updated = await updateTemplate(editing.id, { name, htmlContent: html })
  setItems(items.map((i: TemplateDto) => i.id === editing.id ? updated : i))
      } else {
        const created = await createTemplate({ name, htmlContent: html })
        setItems([created, ...items])
      }
      resetForm()
    } catch (e: any) { setError(e.message) }
  }

  function onEdit(t: TemplateDto) {
    setEditing(t)
    setName(t.name)
    setHtml(t.htmlContent)
  }

  async function onDelete(id: number) {
    if (!confirm('Видалити шаблон?')) return
  try { await deleteTemplate(id); setItems(items.filter((i: TemplateDto) => i.id !== id)) } catch (e:any){ setError(e.message)}
  }

  async function onGenerate(t: TemplateDto) {
    try {
      let data: Record<string,string> = {}
      if (jsonInput.trim()) data = JSON.parse(jsonInput)
      const blob = await generatePdf(t.id, data)
      const url = URL.createObjectURL(blob)
      const a = document.createElement('a')
      a.href = url
      a.download = `${t.name}.pdf`
      document.body.appendChild(a)
      a.click()
      a.remove()
      URL.revokeObjectURL(url)
    } catch (e:any) {
      setError('JSON некоректний або помилка генерації: ' + e.message)
    }
  }

  if (loading) return <div className='container'>Завантаження…</div>
  return (
    <div className='container'>
      <h1>Шаблони</h1>

      {error && <div className='card' style={{background:'#fdecea', borderColor:'#f5c2c7', color:'#58151c'}}>{error}</div>}

      <form className='card' onSubmit={onSubmit}>
        <h3>{editing ? 'Редагувати' : 'Створити новий'}</h3>
        <div style={{display:'grid', gap:8}}>
          <input className='input' placeholder='Назва' value={name} onChange={(e: React.ChangeEvent<HTMLInputElement>)=>setName(e.target.value)} required minLength={2} maxLength={100} />
          <textarea className='input' placeholder='HTML' value={html} onChange={(e: React.ChangeEvent<HTMLTextAreaElement>)=>setHtml(e.target.value)} required rows={6} />
        </div>
        <div className='row' style={{marginTop:8}}>
          <button className='btn primary' type='submit'>{editing ? 'Зберегти' : 'Створити'}</button>
          {editing && <button type='button' className='btn' onClick={resetForm}>Скасувати</button>}
        </div>
      </form>

      <div className='card'>
        <h3>Дані для генерації (JSON)</h3>
        <textarea className='input' rows={6} value={jsonInput} onChange={e=>setJsonInput(e.target.value)} placeholder='{"Number":"01"}' />
      </div>

      <ul className='list'>
        {items.map(t => (
          <li key={t.id} className='card'>
            <div style={{display:'flex', justifyContent:'space-between', alignItems:'center'}}>
              <div>
                <strong>{t.name}</strong>
                <div style={{color:'#57606a', fontSize:12}}>ID: {t.id} · {new Date(t.createdAt).toLocaleString()}</div>
              </div>
              <div className='row'>
                <button className='btn' onClick={()=>onEdit(t)}>Редагувати</button>
                <button className='btn danger' onClick={()=>onDelete(t.id)}>Видалити</button>
                <button className='btn primary' onClick={()=>onGenerate(t)}>Згенерувати PDF</button>
              </div>
            </div>
            <pre style={{whiteSpace:'pre-wrap', marginTop:8}}>{t.htmlContent}</pre>
          </li>
        ))}
      </ul>
    </div>
  )
}
