using Lumina.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Data.Configuration;

public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessages>
{
    public void Configure(EntityTypeBuilder<ChatMessages> builder)
    {

       
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Message)
                   .HasMaxLength(1000) 
                   .IsRequired();   

            builder.Property(x => x.SentAt)
                   .IsRequired();      

            builder.HasOne(x => x.Sender)      
                   .WithMany()              
                   .HasForeignKey(x => x.SenderId)
                   .OnDelete(DeleteBehavior.Restrict); 

         
            builder.HasIndex(x => x.ChatGroupId)
                   .HasDatabaseName("IX_ChatMessage_ChatGroupId");

            builder.HasIndex(x => x.SentAt)
                   .HasDatabaseName("IX_ChatMessage_SentAt");

            builder.HasIndex(x => x.SenderId);
        
    }




















}

