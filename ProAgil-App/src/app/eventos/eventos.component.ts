import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Evento } from '../_models/Evento';
import { EventoService } from '../_services/evento.service';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { ptBrLocale } from 'ngx-bootstrap/locale';
import { BsLocaleService} from 'ngx-bootstrap/datepicker';
import { ToastrService } from 'ngx-toastr';

defineLocale('pt-br',ptBrLocale);

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {
  
  constructor(
    private eventoService : EventoService
    , private modalService: BsModalService
    , private fb: FormBuilder
    , private localeService: BsLocaleService
    , private toastr: ToastrService
    ) { }
    
    _filtroLista: string;
    get filtroLista(): string{
      return this._filtroLista;
    }
    set filtroLista(value: string){
      this._filtroLista = value;
      this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
    }
    
    titulo = 'Eventos';
    
    eventosFiltrados: Evento[];
    eventos: Evento[];
    evento: Evento;
    imagemLargura = 50;
    imagemMargem = 2;
    mostrarImagem = false;
    registerForm: FormGroup;
    modoSalvar = 'post';
    bodyDeletarEvento = '';
    dataEvento : string;
    file: File;
    fileNameUpdate: string;
    dataAtual:string;
    
    
    // tslint:disable-next-line: typedef
    ngOnInit() {
      this.validation();
      this.getEventos();
    }
    
    filtrarEventos(filtrarPor: string): Evento[] {
      filtrarPor = filtrarPor.toLocaleLowerCase();
      return this.eventos.filter(
        evento => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1
        );
      }
      
      alternarImagem(){
        this.mostrarImagem = !this.mostrarImagem;
      }
      
      // tslint:disable-next-line: typedef
      getEventos(){
        this.eventoService.getAllEvento().subscribe(
          (_eventos: Evento[]) => {
            this.eventos = _eventos;
            this.eventosFiltrados = this.eventos;
            console.log(_eventos);
          }, error =>
          {
            this.toastr.error(`Erro ao carregar eventos: ${error}`);
          });
        }
        
        openModal(template: any)
        {
          this.registerForm.reset();
          template.show();
        }
        
        editarEvento(evento: Evento, template:any){
          this.modoSalvar = 'put';
          this.openModal(template);
          this.evento = Object.assign({},evento);
          this.fileNameUpdate = evento.imagemURL.toString();
          this.evento.imagemURL = '';
          this.registerForm.patchValue(this.evento);
        }
        
        novoEvento(template:any){
          this.modoSalvar = 'post';
          this.openModal(template);
        }
        
        
        salvarAlteracao(template: any) {
          if (this.registerForm.valid) {
            if (this.modoSalvar === 'post') {
              this.evento = Object.assign({}, this.registerForm.value);
              
              this.uploadImagem();
              
              this.eventoService.postEvento(this.evento).subscribe(
                (novoEvento: Evento) => {
                  template.hide();
                  this.getEventos();
                  this.toastr.success('Inserido com Sucesso!');
                }, error => {
                  this.toastr.error(`Erro ao Inserir: ${error}`);
                }
                );
              } else {
                this.evento = Object.assign({ id: this.evento.id }, this.registerForm.value);
                
                this.uploadImagem();
                
                this.eventoService.putEvento(this.evento).subscribe(
                  () => {
                    template.hide();
                    this.getEventos();
                    this.toastr.success('Editado com Sucesso!');
                  }, error => {
                    this.toastr.error(`Erro ao Editar: ${error}`);
                  }
                  );
                }
              }
            }
            
            onFileChange(event){
              const reader = new FileReader();
              if(event.target.files && event.target.files.length){
                this.file = event.target.files;
                console.log(this.file);
              }
            }
            
            uploadImagem()
            {
              if(this.modoSalvar === 'post')
              {
                const nomeArquivo = this.evento.imagemURL.split('\\',3);
                this.evento.imagemURL = nomeArquivo[2];
                
                this.eventoService.postUpload(this.file, nomeArquivo[2]).subscribe(
                  () => {
                    this.dataAtual = new Date().getMilliseconds().toString();
                    this.getEventos();
                  }
                  );
                }
                else{
                  this.evento.imagemURL = this.fileNameUpdate;
                  this.fileNameUpdate.split('\\',3);
                  this.eventoService.postUpload(this.file, this.fileNameUpdate).subscribe(
                    () => {
                      this.dataAtual = new Date().getMilliseconds().toString();
                      this.getEventos();
                    }
                    );
                  }
                }
                
                
                excluirEvento(evento: Evento, template: any) {
                  this.openModal(template);
                  this.evento = evento;
                  this.bodyDeletarEvento = `Tem certeza que deseja excluir o Evento: ${evento.tema}, Código: ${evento.id}`;
                }
                
                confirmeDelete(template: any) {
                  this.eventoService.deleteEvento(this.evento.id).subscribe(
                    () => {
                      template.hide();
                      this.getEventos();
                      this.toastr.success('Deletado com sucesso');
                    }, error => {
                      this.toastr.error('Erro ao exlcuir');
                      console.log(error);
                    }
                    );
                  }
                  
                  
                  
                  validation(){
                    this.registerForm = this.fb.group({
                      tema: ['',[Validators.required,Validators.minLength(4),Validators.maxLength(50)]],
                      local: ['',Validators.required],
                      dataEvento: ['',Validators.required],
                      telefone: ['',[Validators.required]],
                      email: ['',[Validators.required,Validators.email]],
                      qtdPessoas: ['',[Validators.required,Validators.max(120000)]],
                      imagemURL: ['',Validators.required]
                    });
                  }
                  
                }
                