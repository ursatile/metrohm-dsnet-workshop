# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: example.proto

require 'google/protobuf'

Google::Protobuf::DescriptorPool.generated_pool.build do
  add_file("example.proto", :syntax => :proto3) do
    add_message "example.HelloRequest" do
      optional :name, :string, 1
      optional :language, :string, 2
    end
    add_message "example.HelloReply" do
      optional :greeting, :string, 1
    end
  end
end

module Example
  HelloRequest = ::Google::Protobuf::DescriptorPool.generated_pool.lookup("example.HelloRequest").msgclass
  HelloReply = ::Google::Protobuf::DescriptorPool.generated_pool.lookup("example.HelloReply").msgclass
end